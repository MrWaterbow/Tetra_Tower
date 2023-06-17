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

        private IBrickViewPresenter _presenter;
        private ITileViewFactory _tileFactory;

        private List<BrickTileView> _tiles;

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
            foreach (BrickTileView tileView in _tiles)
            {
                tileView.SetColor(color);
            }
        }

        public IReadOnlyList<IReadOnlyBrickTileView> Tiles => _tiles;
        public Mesh Mesh => _prefab.Mesh;
        public Color GeneralColor => Color.black;//_tiles[0].Color;

        public void SetCallbacks(IBrickViewPresenter presenter)
        {
            presenter.OnPositionChanged += ChangePosition;

            _presenter = presenter;
        }

        public void SetCallbacks()
        {
            _presenter.OnPositionChanged += ChangePosition;
        }

        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
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

        private Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Length)];
        }
    }
}