using Server.BrickLogic;
using UnityEngine;

namespace Server.Factories
{
    /// <summary>
    /// Интерфейс фабрики для создания блоков.
    /// </summary>
    public interface IBrickFactory
    {
        /// <summary>
        /// Метод создания блока.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Brick Create(Vector3Int position);
    }
}