using Server.BrickLogic;
using System;
using UnityEngine;

namespace Server.GhostLogic
{
    public class GhostViewPresenter : IGhostViewPresenter
    {
        public event Action<Vector3> OnPositionChanged;

        private readonly IReadOnlyBricksDatabase _database;

        public GhostViewPresenter(IReadOnlyBricksDatabase database)
        {
            _database = database;
        }

        public void SetAndInvokeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged += InvokeOnPositionChanged;

            OnPositionChanged?.Invoke(GetWorldPosition());
        }

        public void DisposeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged -= InvokeOnPositionChanged;
        }

        /// <summary>
        /// Метод вызывается когда позиция блока меняется
        /// </summary>
        /// <param name="position"></param>
        private void InvokeOnPositionChanged(Vector3Int position)
        {
            OnPositionChanged?.Invoke(GetWorldPosition());
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