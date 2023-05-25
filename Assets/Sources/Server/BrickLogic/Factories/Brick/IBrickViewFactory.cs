using Server.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public interface IBrickViewFactory
    {
        /// <summary>
        /// Создание блока на сцене
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        BrickView Create(Vector3 position);
    }
}