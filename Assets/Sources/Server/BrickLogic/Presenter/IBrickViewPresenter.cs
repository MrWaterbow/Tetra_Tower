using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IBrickViewPresenter
    {
        /// <summary>
        /// Вызывается когда позиция меняется
        /// </summary>
        event Action<Vector3> OnPositionChanged;

        void SetCallbacks();
        void DisposeCallbacks();
    }
}