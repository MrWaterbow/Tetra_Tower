using DG.Tweening;
using Server.BrickLogic;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Client.BrickLogic
{
    internal sealed class BrickView : MonoBehaviour, IReadOnlyBrickView
    {
        /// <summary>
        /// Transform ( компонент Unity )
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

        private IControllableBrickViewPresenter _controllablePresenter;
        private IBrickViewPresenter _brickPresenter;
        private ITileViewFactory _tileFactory;

        private List<BrickTileView> _tiles;
        private Color _generalColor;

        public void Initialize(Vector3Int[] pattern)
        {
            _tileFactory = new TileViewFactory(_prefab, _transform);
            _tiles = new();

            CreateBlockByTiles(pattern);
            SetTilesColor(GetRandomColor());
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

        /// <summary>
        /// Уничтожение блока и отписка от presenter
        /// </summary>
        private void Destroy()
        {
            Debug.Log("Brick destroying");

            DisposeCallbacks();
            ActiveTilesRigidbodies();
        }

        private void ActiveTilesRigidbodies()
        {
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.ActiveRigidbody();
                tileView.AddForceToRigidbody();
            }

            Destroy(gameObject, _destroyTimer);
        }

        private Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Length)];
        }
    }
}