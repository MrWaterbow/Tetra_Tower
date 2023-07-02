using Server.BrickLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickView : MonoBehaviour, IReadOnlyBrickView
    {
        /// <summary>
        /// Transform (компонент Unity)
        /// </summary>
        [SerializeField] private Transform _transform;
        [SerializeField] private BrickTileView _prefab;

        [Space]

        [SerializeField] private Color[] _colors;
        /// <summary>
        /// Плавность сменения позиции
        /// </summary>
        [SerializeField] private float _changePositionSmoothTime;
        [SerializeField] private float _destroyTimer;
        [SerializeField] private float _unstableTimer;

        private PlacingSurface _placingSurface;
        private IReadOnlyBrick _brick;
        private ITileViewFactory _tileFactory;

        private List<BrickTileView> _tiles;
        private Color _generalColor;

        private Color _randomColor;

        private bool _unstableEffect;

        public void Initialize(IReadOnlyCollection<Vector3Int> pattern)
        {
            _tileFactory = new TileViewFactory(_prefab, _transform);
            _tiles = new();

            CreateBlockByTiles(pattern);

            _randomColor = GetRandomColor();
            SetTilesColor(_randomColor);
        }

        /// <summary>
        /// Получение случайного цвета
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Length)];
        }

        private void CreateBlockByTiles(IReadOnlyCollection<Vector3Int> pattern)
        {
            foreach (Vector3Int tile in pattern)
            {
                _tiles.Add(_tileFactory.Create(tile));
            }
        }

        private void SetTilesColor(Color color)
        {
            _generalColor = color;

            foreach (BrickTileView tileView in _tiles)
            {
                tileView.SetColor(color);
            }
        }

        public IReadOnlyList<IReadOnlyBrickTileView> Tiles => _tiles;
        public Mesh Mesh => _prefab.Mesh;
        public Color GeneralColor => _generalColor;

        public void SetCallbacks(IReadOnlyBrick brick, PlacingSurface placingSurface)
        {
            _placingSurface = placingSurface;
            _brick = brick;

            _brick.OnPositionChanged += ChangePosition;
            _brick.OnRotate90 += Rotate90;

            _brick.OnTileRemoved += UpdatePattern;
            _brick.UnstableWarning += UpdateUnstableEffect;
            _brick.OnDestroy += Destroy;

            ChangePosition(brick.Position);
        }

        /// <summary>
        /// Отписка от presenter'ов
        /// </summary>
        public void DisposeCallbacks()
        {
            _brick.OnPositionChanged -= ChangePosition;
            _brick.OnRotate90 -= Rotate90;
        }

        /// <summary>
        /// Изменяет позицию блока ( использует DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangePosition(Vector3Int newPosition)
        {
            Vector3 worldPosition = _placingSurface.GetWorldPosition(newPosition);

            _transform.position = worldPosition;
        }

        private void Rotate90(IReadOnlyCollection<Vector3Int> pattern)
        {
#if UNITY_EDITOR
            UpdatePattern(pattern);
#else
            _transform.Rotate(Vector3.up, 90);
#endif
        }

        private void UpdatePattern(IReadOnlyCollection<Vector3Int> pattern)
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.KillLoopUnstableEffect();
                Destroy(tileView.gameObject);
            }

            _tiles.Clear();

            CreateBlockByTiles(pattern);
            SetTilesColor(_randomColor);
        }

        private void UpdateUnstableEffect(bool unstableWarning)
        {
            if (unstableWarning && _unstableEffect) return;

            if(unstableWarning)
            {
                _unstableEffect = true;
                PlayUnstableEffect();
            }
            else
            {
                _unstableEffect = false;
                KillUnstableEffect();
            }
        }

        private void PlayUnstableEffect()
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.PlayLoopUnstableEffect();
            }
        }

        private void KillUnstableEffect()
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.KillLoopUnstableEffect();
            }
        }

        /// <summary>
        /// Уничтожение блока и отписка от presenter
        /// </summary>
        private void Destroy()
        {
            DisposeCallbacks();
            ActiveTilesRigidbodies();
        }

        /// <summary>
        /// Активирует rigidbody для тайлов
        /// </summary>
        private void ActiveTilesRigidbodies()
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.ActiveRigidbody();
                tileView.AddForceToRigidbody();
            }

            Destroy(gameObject, _destroyTimer);
        }
    }
}