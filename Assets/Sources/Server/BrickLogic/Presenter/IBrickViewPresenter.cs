using System;

namespace Server.BrickLogic
{
    public interface IBrickViewPresenter
    {
        event Action<bool> UnstableWarning;
        event Action OnDestroy;

        /// <summary>
        /// Подписывается и вызывает нужные ивенты.
        /// </summary>
        void SetCallbacks();

        /// <summary>
        /// Отписывается от ивентов.
        /// </summary>
        void DisposeCallbacks();
    }
}