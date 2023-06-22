using System;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Класс, который представляет сущность блока.
    /// </summary>
    public class Brick : IReadOnlyBrick
    {
        /// <summary>
        /// Ивент вызывается при смене позиции блока.
        /// </summary>
        public event Action<Vector3Int> OnPositionChanged;
        public event Action<bool> UnstableWarning;
        public event Action OnDestroy;

        /// <summary>
        /// Позиция блока.
        /// </summary>
        private Vector3Int _position;
        /// <summary>
        /// Массив кубиков из которых состоит блок.
        /// </summary>
        private Vector3Int[] _pattern;

        /// <summary>
        /// //Конструктор позволяет назначить position и pattern блока.
        /// </summary>
        /// <param name="position">Позиция блока</param>
        /// <param name="pattern">Массив кубиков из которых состоит блок</param>
        public Brick(Vector3Int position, Vector3Int[] pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        /// <summary>
        /// Возвращает копию позиции блока.
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// Возвращает копию паттерна блока.
        /// </summary>
        public Vector3Int[] Pattern => _pattern;

        /// <summary>
        /// Двигает блок в указаном направлении.
        /// </summary>
        /// <param name="direction">Направление</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;

            OnPositionChanged?.Invoke(_position);
        }

        /// <summary>
        /// Меняет позицию блока.
        /// </summary>
        /// <param name="position"></param>
        public void ChangePosition(Vector3Int position)
        {
            _position = position;

            OnPositionChanged?.Invoke(_position);
        }

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}