﻿using System;
using System.Collections.Generic;
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
        event Action<IReadOnlyCollection<Vector3Int>> OnRotate90;
        event Action<IReadOnlyCollection<Vector3Int>> OnTileRemoved;

        event Action OnDestroy;
        event Action<bool> UnstableWarning;

        /// <summary>
        /// Возвращает копию позиции блока.
        /// </summary>
        Vector3Int Position { get; }
        /// <summary>
        /// Возвращает копию паттерна блока.
        /// </summary>
        IReadOnlyCollection<Vector3Int> Pattern { get; }

        bool UnstableEffect { get; }
    }
}