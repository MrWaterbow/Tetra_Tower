﻿using Server.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public sealed class BrickViewFactory : IBrickViewFactory
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
        public BrickView Create(Vector3 position)
        {
            return Object.Instantiate(_prefab, position, Quaternion.identity);
        }
    }
}