using System;

namespace Server.BrickLogic
{
    public sealed class BrickViewPresenter : IBrickViewPresenter
    {
        public event Action OnDestroy;

        /// <summary>
        /// Чтение данных из базы данных.
        /// </summary>
        private readonly IReadOnlyBrick _brick;

        public BrickViewPresenter(IReadOnlyBrick brick)
        {
            _brick = brick;
        }

        /// <summary>
        /// Подписывается и вызывает нужные ивенты.
        /// </summary>
        public void SetCallbacks()
        {
            _brick.OnDestroy += OnDestroy.Invoke;
        }

        /// <summary>
        /// Отписывается от ивентов.
        /// </summary>
        public void DisposeCallbacks()
        {
            _brick.OnDestroy -= OnDestroy.Invoke;
        }
    }
}