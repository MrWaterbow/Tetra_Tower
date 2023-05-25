using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IBrickInputView
    {
        event Action<Vector3Int> OnMove;
        event Action OnLower;

        /// <summary>
        /// Подписывается на ивенты от ввода игрока
        /// </summary>
        void SetCallbacks();
        /// <summary>
        /// Отписывается от ивентов для ввода игрока
        /// </summary>
        void DisposeCallbacks();
    }
}