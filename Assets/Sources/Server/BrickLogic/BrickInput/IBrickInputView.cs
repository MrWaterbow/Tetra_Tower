using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IBrickInputView
    {
        event Action<Vector3Int> OnMove;
        event Action OnLower;

        void SetCallbacks();
        void DisposeCallbacks();
    }
}