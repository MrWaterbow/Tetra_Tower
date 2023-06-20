using System;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Интерфейс для доступа к чтению данных из блока.
    /// </summary>
    public interface IReadOnlyBrick
    {
        /// <summary>
        /// Ивент, который вызывается при смене позиции блока.
        /// </summary>
        event Action<Vector3Int> OnPositionChanged;
        event Action OnDestroy;

        /// <summary>
        /// Возвращает копию позиции блока.
        /// </summary>
        Vector3Int Position { get; }
        /// <summary>
        /// Возвращает копию паттерна блока.
        /// </summary>
        Vector3Int[] Pattern { get; }
    }
}