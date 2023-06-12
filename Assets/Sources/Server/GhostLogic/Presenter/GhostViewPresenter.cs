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

            OnPositionChanged?.Invoke(GetWorldPosition(_database.ControllableBrick.Position));
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
            OnPositionChanged?.Invoke(GetWorldPosition(position));
        }

        private Vector3 GetWorldPosition(Vector3Int position)
        {
            position *= new Vector3Int(1, 0, 1);
            Vector3 worldPosition = _database.Surface.GetWorldPosition(position);

            worldPosition += new Vector3(0.5f, 0, 0.5f);

            return worldPosition;
        }
    }
}