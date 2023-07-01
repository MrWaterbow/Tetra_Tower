using System.Collections.Generic;
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
        BrickView Create(Vector3 position, IReadOnlyCollection<Vector3Int> pattern);
    }
}