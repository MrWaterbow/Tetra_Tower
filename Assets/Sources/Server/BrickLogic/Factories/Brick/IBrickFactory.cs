using Database.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public interface IBrickFactory
    {
        /// <summary>
        /// Абстракция для создания блока
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Brick Create(Vector3Int position);
    }
}