using System;
using UnityEngine;

namespace Server.BricksLogic.GhostLogic
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
            OnPositionChanged?.Invoke(_bricksSpace.Surface.GetWorldPosition(position));
        }
    }
}