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
        public event Action<Vector3Int[]> OnRotate90;

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
            _database.ControllableBrick.OnRotate90 += InvokeOnRotate90;

            OnPositionChanged?.Invoke(GetWorldPosition(_database.ControllableBrick.Position));
        }

        /// <summary>
        /// Отписывается от ивентов
        /// </summary>
        public void DisposeCallbacks()
        {
            _database.ControllableBrick.OnPositionChanged -= InvokeOnPositionChanged;
            _database.ControllableBrick.OnRotate90 -= InvokeOnRotate90;
        }

        /// <summary>
        /// Метод вызывается когда позиция блока меняется
        /// </summary>
        /// <param name="position"></param>
        private void InvokeOnPositionChanged(Vector3Int position)
        {
            OnPositionChanged?.Invoke(GetWorldPosition(position));
        }

        private void InvokeOnRotate90(Vector3Int[] pattern)
        {
            OnRotate90?.Invoke(pattern);
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