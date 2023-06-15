using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Доступ к чтению данных из базы данных, а также методы для получения значений.
    /// </summary>
    public interface IReadOnlyBricksDatabase
    {
        /// <summary>
        /// Доступ к чтению карты высот.
        /// </summary>
        IReadOnlyDictionary<Vector2Int, int> HeightMap { get; }
        /// <summary>
        /// Доступ к чтению списка поставленных блоков.
        /// </summary>
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }
        /// <summary>
        /// Доступ к чтению данных из контролируемого блока.
        /// </summary>
        IReadOnlyBrick ControllableBrick { get; }
        /// <summary>
        /// Возвращает копию поверхности на которую ставятся блоки.
        /// </summary>
        PlacingSurface Surface { get; }

        /// <summary>
        /// Возвращает высоту по ключу (позиции).
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        int GetHeightByKey(Vector2Int position);
        /// <summary>
        /// Возвращает высоту по паттерну блока.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        int GetHeightByPattern(IReadOnlyBrick brick);
        /// <summary>
        /// Возвращает мировую позицию контролируемого блока.
        /// </summary>
        /// <returns></returns>
        Vector3 GetControllableBrickWorldPosition();
        /// <summary>
        /// Поставлен ли блок на землю.
        /// </summary>
        /// <returns></returns>
        bool ControllableBrickOnGround();
        /// <summary>
        /// Возвращает наивысшую точку (высоту).
        /// </summary>
        /// <returns></returns>
        int GetHeighestPoint();
    }
}