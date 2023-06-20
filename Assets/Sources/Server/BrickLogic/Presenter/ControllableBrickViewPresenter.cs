using System;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class ControllableBrickViewPresenter : IControllableBrickViewPresenter
    {
        /// <summary>
        /// Вызывается, если позиция блока сменилась.
        /// </summary>
        public event Action<Vector3> OnPositionChanged;

        /// <summary>
        /// Чтение данных из базы данных.
        /// </summary>
        private readonly IReadOnlyBricksDatabase _database;

        public ControllableBrickViewPresenter(IReadOnlyBricksDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Подписывается на ивенты
        /// </summary>
        /// <param name="brick"></param>
        public void SetAndInvokeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged += InvokeOnPositionChanged;

            OnPositionChanged?.Invoke(GetWorldPosition(_database.ControllableBrick.Position));
        }

        /// <summary>
        /// Отписывается от ивентов
        /// </summary>
        public void DisposeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged -= InvokeOnPositionChanged;
        }

        /// <summary>
        /// Метод вызывается когда позиция блока меняется
        /// </summary>
        /// <param name="position"></param>
        private void InvokeOnPositionChanged(Vector3Int position)
        {
            OnPositionChanged?.Invoke(GetWorldPosition(position));
        }

        /// <summary>
        /// Получить мировую позицию
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3 GetWorldPosition(Vector3Int position)
        {
            return _database.Surface.GetWorldPosition(position);
        }
    }
}