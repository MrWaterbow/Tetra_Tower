﻿using System;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BrickViewPresenter : IBrickViewPresenter
    {
        public event Action<Vector3> OnPositionChanged;

        private readonly IReadOnlyBricksDatabase _database;

        public BrickViewPresenter(IReadOnlyBricksDatabase database)
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

        private Vector3 GetWorldPosition(Vector3Int position)
        {
            return _database.Surface.GetWorldPosition(position);
        }
    }
}