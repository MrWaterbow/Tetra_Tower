using System;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Интерфейс для отображения визуальной составляющей блока.
    /// </summary>
    public interface IControllableBrickViewPresenter
    {
        /// <summary>
        /// Вызывается когда позиция меняется.
        /// </summary>
        event Action<Vector3> OnPositionChanged;
        event Action<Vector3Int[]> OnRotate90;

        /// <summary>
        /// Подписывается и вызывает нужные ивенты.
        /// </summary>
        void SetAndInvokeCallbacks();

        /// <summary>
        /// Отписывается от ивентов.
        /// </summary>
        void DisposeCallbacks();
    }
}