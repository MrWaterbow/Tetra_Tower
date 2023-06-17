using UnityEngine;

namespace Client.BrickLogic
{
    internal interface IBrickViewFactory
    {
        /// <summary>
        /// Создание блока на сцене
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        BrickView Create(Vector3 position, Vector3Int[] pattern);
    }
}