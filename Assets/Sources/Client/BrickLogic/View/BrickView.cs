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

        private IControllableBrickViewPresenter _controllablePresenter;
        private IBrickViewPresenter _brickPresenter;
        private ITileViewFactory _tileFactory;

        private List<BrickTileView> _tiles;
        private Color _generalColor;

        private bool _unstableEffect;

        public void Initialize(Vector3Int[] pattern)
        {
            _tileFactory = new TileViewFactory(_prefab, _transform);
            _tiles = new();

            CreateBlockByTiles(pattern);
            SetTilesColor(GetRandomColor());
        }

        /// <summary>
        /// Получение случайного цвета
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Length)];
        }

        private void CreateBlockByTiles(Vector3Int[] pattern)
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

        public void SetCallbacks(IControllableBrickViewPresenter controllablePresenter,
            IBrickViewPresenter brickPresenter)
        {
            controllablePresenter.OnPositionChanged += ChangePosition;
            controllablePresenter.OnRotate90 += Rotate90;

            brickPresenter.UnstableWarning += UpdateUnstableEffect;
            brickPresenter.OnDestroy += Destroy;

            _controllablePresenter = controllablePresenter;
            _brickPresenter = brickPresenter;
        }

        /// <summary>
        /// Отписка от presenter'ов
        /// </summary>
        public void DisposeCallbacks()
        {
            _controllablePresenter.OnPositionChanged -= ChangePosition;
            _controllablePresenter.OnRotate90 -= Rotate90;

            _brickPresenter.UnstableWarning -= UpdateUnstableEffect;
            _brickPresenter.OnDestroy -= Destroy;
        }

        /// <summary>
        /// Изменяет позицию блока ( использует DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangePosition(Vector3 newPosition)
        {
            _transform.position = newPosition;
            //_transform.DOMove(newPosition, _changePositionSmoothTime);
        }

        private void Rotate90(Vector3Int[] pattern)
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.KillLoopUnstableEffect();
                Destroy(tileView.gameObject);
            }

            _tiles.Clear();

            CreateBlockByTiles(pattern);
            SetTilesColor(GetRandomColor());

            //print("Rotating");

            //_transform.Rotate(Vector3.up, 90);
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