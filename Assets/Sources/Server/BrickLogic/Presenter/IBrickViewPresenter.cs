using System;
using UnityEngine;

namespace Server.BrickLogic
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