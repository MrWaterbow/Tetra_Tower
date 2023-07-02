using System;
using System.Collections.Generic;
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
        public event Action<IReadOnlyCollection<Vector3Int>> OnRotate90;
        public event Action<IReadOnlyCollection<Vector3Int>> OnTileRemoved;

        public event Action<bool> UnstableWarning;
        public event Action OnDestroy;

        /// <summary>
        /// Позиция блока.
        /// </summary>
        private Vector3Int _position;
        /// <summary>
        /// Массив кубиков из которых состоит блок.
        /// </summary>
        private HashSet<Vector3Int> _pattern;
        private Vector3Int[][] _patternRotation;
        private int _rotationCounter;
        private readonly int _rotationCounterBorder;

        private bool _unstableEffect;

        private Brick(Vector3Int position)
        {
            _position = position;
        }

        /// <summary>
        /// //Конструктор позволяет назначить tileWorldPosition и pattern блока.
        /// </summary>
        /// <param name="position">Позиция блока</param>
        /// <param name="pattern">Массив кубиков из которых состоит блок</param>
        public Brick(Vector3Int position, HashSet<Vector3Int> pattern, Vector3Int[][] rotationPattern) : this(position)
        {
            _pattern = new();
            foreach (Vector3Int tile in pattern)
            {
                _pattern.Add(tile);
            }

            _patternRotation = rotationPattern;
            _rotationCounter = 1;
            _rotationCounterBorder = rotationPattern.Length;
        }

        public Brick(Vector3Int position, BrickBlank blank) : 
            this(position, blank.Pattern, blank.PatternRotation)
        {

        }

        /// <summary>
        /// Возвращает копию позиции блока.
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// Возвращает копию паттерна блока.
        /// </summary>
        public IReadOnlyCollection<Vector3Int> Pattern => _pattern;

        public bool UnstableEffect => _unstableEffect;

        /// <summary>
        /// Двигает блок в указаном направлении.
        /// </summary>
        /// <param name="direction">Направление</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;

            OnPositionChanged?.Invoke(_position);
        }

        public void Rotate90()
        {
            if (_rotationCounterBorder == 1) return;

            if(_rotationCounter == _rotationCounterBorder)
            {
                _rotationCounter = 1;
            }
            else
            {
                _rotationCounter++;
            }

            _pattern.Clear();

            foreach (Vector3Int tile in _patternRotation[_rotationCounter - 1])
            {
                _pattern.Add(tile);
            }

            OnRotate90?.Invoke(_pattern);
            OnPositionChanged?.Invoke(_position);
        }
        
        public Vector3Int[] GetFeatureRotatePattern()
        {
            if(_rotationCounter == _rotationCounterBorder)
            {
                return _patternRotation[0];
            }
            else
            {
                return _patternRotation[_rotationCounter];
            }
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

        public void RemoveTile(Vector3Int tileWorldPosition)
        {
            Vector3Int tilePosition = tileWorldPosition - _position;

            if(_pattern.Remove(tilePosition) == false)
            {
                throw new NullReferenceException("Tile not found");
            }

            OnTileRemoved?.Invoke(_pattern);
        }

        public void InvokeUnstableWarning(bool value)
        {
            if (_unstableEffect && value == false)
            {
                UnstableWarning?.Invoke(value);
            }
            else if(_unstableEffect == false && value)
            {
                UnstableWarning?.Invoke(value);
            }

            _unstableEffect = value;
        }
    }
}