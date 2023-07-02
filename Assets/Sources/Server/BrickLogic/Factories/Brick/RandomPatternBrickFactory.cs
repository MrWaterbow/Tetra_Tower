using Server.BrickLogic;
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
        private readonly BrickBlank[] _patterns;
        private readonly Vector3Int _startPosition;

        private readonly IReadOnlyBricksDatabase _database;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patterns">Массив паттернов для блока</param>
        public RandomPatternBrickFactory(BrickBlank[] patterns, Vector3Int startPosition, IReadOnlyBricksDatabase database)
        {
            _patterns = patterns;
            _startPosition = startPosition;
            _database = database;
        }

        /// <summary>
        /// Метод для создания блока со случайным паттерном
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Brick Create()
        {
            Vector3Int position = _startPosition;

            position.y = _database.GetHeighestPoint();
            position.y += _startPosition.y;

            BrickBlank randomBlank = _patterns[Random.Range(0, _patterns.Length)];

            return new Brick(position, randomBlank);
        }
    }
}