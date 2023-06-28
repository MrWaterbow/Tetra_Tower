using Server.BrickLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostView : MonoBehaviour
    {
        [SerializeField] private GhostTileView _prefab;
        [SerializeField] private Transform _transform;

        private IReadOnlyBricksDatabase _database;
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
        public void SetCallbacks(IReadOnlyBricksDatabase database)
        {
            _database = database;

            _database.ControllableBrick.OnPositionChanged += ChangePosition;
            _database.ControllableBrick.OnRotate90 += Rotate90;

            ChangePosition(_database.ControllableBrick.Position);
        }

        /// <summary>
        /// Отписывается от ивентов от презентера.
        /// </summary>
        public void DisposeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged -= ChangePosition;
            _database.ControllableBrick.OnRotate90 -= Rotate90;
        }

        /// <summary>
        /// Изменяет позицию блока (использует DOTween).
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangePosition(Vector3Int newPosition)
        {
            newPosition.y = _database.GetHeightByBlock(_database.ControllableBrick);
            Vector3 worldPosition = _database.Surface.GetWorldPosition(newPosition);

            _transform.position = worldPosition;
        }

        private void Rotate90(Vector3Int[] pattern)
        {
            _transform.Rotate(Vector3.up, 90);
        }

        public void RefreshTransform()
        {
            _transform.eulerAngles = Vector3.zero;
        }
    }
}