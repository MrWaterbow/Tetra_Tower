using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BrickViewPresenter : IBrickViewPresenter
    {
        public event Action<Vector3> OnPositionChanged;

        private readonly BricksSpace _bricksSpace;

        private Brick _brick;

        public BrickViewPresenter(BricksSpace bricksSpace)
        {
            _bricksSpace = bricksSpace;
        }

        /// <summary>
        /// Подписывается на ивенты
        /// </summary>
        /// <param name="brick"></param>
        public void SetCallbacks(Brick brick)
        {
            brick.OnPositionChanged += InvokeOnPositionChanged;

            _brick = brick;
        }

        /// <summary>
        /// Отписывается от ивентов
        /// </summary>
        public void DisposeCallbacks()
        {
            if (_brick == null) return;

            _brick.OnPositionChanged -= InvokeOnPositionChanged;
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