using Server.BrickLogic;
using System;
using UnityEngine;

namespace Server.GhostLogic
{
    public class GhostViewPresenter : IGhostViewPresenter
    {
        public event Action<Vector3> OnPositionChanged;

        private readonly BricksSpace _bricksSpace;

        public GhostViewPresenter(BricksSpace bricksSpace)
        {
            _bricksSpace = bricksSpace;
        }

        public void SetCallbacks()
        {
            _bricksSpace.ControllableBrick.OnPositionChanged += InvokeOnPositionChanged;
        }

        public void DisposeCallbacks()
        {
            _bricksSpace.ControllableBrick.OnPositionChanged -= InvokeOnPositionChanged;
        }

        /// <summary>
        /// Метод вызывается когда позиция блока меняется
        /// </summary>
        /// <param name="position"></param>
        private void InvokeOnPositionChanged(Vector3Int position)
        {
            Vector3 worldPosition = _bricksSpace.Surface.GetWorldPosition(position * new Vector3Int(1, 0, 1));

            OnPositionChanged?.Invoke(worldPosition);
        }
    }
}