using System;
using UnityEngine;

namespace Server.GhostLogic
{
    public interface IGhostViewPresenter
    {
        /// <summary>
        /// Вызывается когда позиция меняется
        /// </summary>
        event Action<Vector3> OnPositionChanged;

        void SetCallbacks();
        void DisposeCallbacks();
    }
}