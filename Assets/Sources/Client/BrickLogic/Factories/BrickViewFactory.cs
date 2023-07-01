using System.Collections.Generic;
using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickViewFactory : IBrickViewFactory
    {
        /// <summary>
        /// префаб блока
        /// </summary>
        private readonly BrickView _prefab;

        public BrickViewFactory(BrickView prefab)
        {
            _prefab = prefab;
        }

        /// <summary>
        /// Создает блок на сцене
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public BrickView Create(Vector3 position, IReadOnlyCollection<Vector3Int> pattern)
        {
            BrickView instance = Object.Instantiate(_prefab, position, Quaternion.identity);

            instance.Initialize(pattern);

            return instance;
        }
    }
}