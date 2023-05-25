using Server.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    /// <summary>
    /// Реализация создания блока со случайным паттернов
    /// </summary>
    public sealed class RandomPatternBrickFactory : IBrickFactory
    {
        /// <summary>
        /// Массив паттернов для блока
        /// </summary>
        private readonly Vector3Int[][] _patterns;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patterns">Массив паттернов для блока</param>
        public RandomPatternBrickFactory(Vector3Int[][] patterns)
        {
            _patterns = patterns;
        }

        /// <summary>
        /// Метод для создания блока со случайным паттерном
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public IBrick Create(Vector3Int position)
        {
            return new Brick(position, _patterns[Random.Range(0, _patterns.Length)]);
        }
    }
}