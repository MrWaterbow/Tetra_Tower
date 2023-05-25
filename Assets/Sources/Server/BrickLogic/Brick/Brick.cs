using UnityEngine;

namespace Server.BricksLogic
{
    /// <summary>
    ///Реализует интерфейс IBrick, он позволяет реализовать логику манипуляций с блоком
    /// </summary>
    public sealed class Brick : IBrick
    {
        /// <summary>
        /// Позиция блока
        /// </summary>
        private Vector3Int _position;
        /// <summary>
        /// Массив кубиков из которых состоит блок
        /// </summary>
        private Vector3Int[] _pattern;

        /// <summary>
        /// //Конструктор позволяет назначить position и pattern блока
        /// </summary>
        /// <param name="position">Позиция блока</param>
        /// <param name="pattern">Массив кубиков из которых состоит блок</param>
        public Brick(Vector3Int position, Vector3Int[] pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        // Свойства, здесь получаем значения для использования из вне, т.е. в других модулях/классах и пр.
        /// <summary>
        /// Доступ к чтению позиции блока
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// Доступ к чтению массива кубиков, из которых состоит блок
        /// </summary>
        public Vector3Int[] Pattern => _pattern;

        /// <summary>
        /// Низкоуровневая Бизнесс правило, это то правило которое работает с сущностью блока
        /// </summary>
        /// <param name="direction">Направление</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;
        }
    }
}