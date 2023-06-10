using System;
using UnityEngine;

namespace Server.BrickLogic
{

    /// <summary>
    /// Класс, которая представляет сущность блока
    /// </summary>
    public class Brick : IReadOnlyBrick
    {
        public event Action<Vector3Int> OnPositionChanged;

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

        public Vector3Int Position => _position;
        public Vector3Int[] Pattern => _pattern;

        /// <summary>
        /// Низкоуровневая Бизнесс правило, это то правило которое работает с сущностью блока
        /// </summary>
        /// <param name="direction">Направление</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;

            OnPositionChanged?.Invoke(_position);
        }
    }
}