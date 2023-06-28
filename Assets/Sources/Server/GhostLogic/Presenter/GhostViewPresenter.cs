using Server.BrickLogic;
using System;
using UnityEngine;

namespace Server.GhostLogic
{
    public class GhostViewPresenter : IGhostViewPresenter
    {
        public event Action<Vector3> OnPositionChanged;
        public event Action<Vector3Int[]> OnRotate90;

        private readonly IReadOnlyBricksDatabase _database;

        public GhostViewPresenter(IReadOnlyBricksDatabase database)
        {
            _database = database;
        }

        public void SetAndInvokeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged += InvokeOnPositionChanged;
            _database.ControllableBrick.OnRotate90 += InvokeRotate90;

            OnPositionChanged?.Invoke(GetWorldPosition());
        }

        public void DisposeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged -= InvokeOnPositionChanged;
            _database.ControllableBrick.OnRotate90 -= InvokeRotate90;
        }

        /// <summary>
        /// Метод вызывается когда позиция блока меняется
        /// </summary>
        /// <param name="position"></param>
        private void InvokeOnPositionChanged(Vector3Int position)
        {
            OnPositionChanged?.Invoke(GetWorldPosition());
        }

        private void InvokeRotate90(Vector3Int[] pattern)
        {
            OnRotate90?.Invoke(pattern);
        }

        private Vector3 GetWorldPosition()
        {
            Vector3Int localPosition = _database.ControllableBrick.Position;
            localPosition.y = _database.GetHeightByBlock(_database.ControllableBrick);
            Vector3 worldPosition = _database.Surface.GetWorldPosition(localPosition);

            return worldPosition;
        }
    }
}