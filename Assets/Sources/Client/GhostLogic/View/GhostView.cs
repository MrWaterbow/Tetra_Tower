using Client.BrickLogic;
using Server.GhostLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostView : MonoBehaviour
    {
        [SerializeField] private GhostTileView _prefab;
        [SerializeField] private Transform _transform;

        private IGhostViewPresenter _presenter;
        private IGhostTileViewFactory _tileFactory;

        private List<GhostTileView> _tiles;

        public void Initialize(Vector3Int[] pattern, Color color)
        {
            ClearTiles();

            _tileFactory = new GhostTileViewFactory(_prefab, _transform);
            _tiles = new();

            CreateBlockByTiles(pattern);
            SetTilesColor(color);
        }

        private void ClearTiles()
        {
            if (_tiles == null) return;

            foreach (GhostTileView tileView in _tiles)
            {
                tileView.Destroy();
            }

            _tiles.Clear();
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
            foreach (GhostTileView tileView in _tiles)
            {
                tileView.SetColor(color);
            }
        }

        /// <summary>
        /// Подписывается на ивенты от презентера при этом назначая его
        /// </summary>
        /// <param name="presenter"></param>
        public void SetCallbacks(IGhostViewPresenter presenter)
        {
            presenter.OnPositionChanged += ChangePosition;

            _presenter = presenter;
        }

        /// <summary>
        /// Подписывается на ивенты от презентера.
        /// </summary>
        public void SetCallbacks()
        {
            _presenter.OnPositionChanged += ChangePosition;
        }

        /// <summary>
        /// Отписывается от ивентов от презентера.
        /// </summary>
        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
        }

        /// <summary>
        /// Изменяет позицию блока (использует DOTween).
        /// </summary>
        /// <param name="newPosition"></param>
        public void ChangePosition(Vector3 newPosition)
        {
            _transform.position = newPosition;
        }

        ///// <summary>
        ///// Меняет меш призрака.
        ///// </summary>
        ///// <param name="mesh"></param>
        //public void SetMesh(Mesh mesh)
        //{
        //    _meshFilter.mesh = mesh;
        //}

        ///// <summary>
        ///// Меняет цвет призрака.
        ///// </summary>
        ///// <param name="color"></param>
        //public void SetColor(Color color)
        //{
        //    _meshRenderer.material.color = color;
        //}
    }
}