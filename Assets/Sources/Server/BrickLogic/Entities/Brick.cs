using System;
using System.Drawing.Drawing2D;
using UnityEngine;
using UnityEngine.UIElements;

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
        private int[,] _matrix;
        private int _matrixColumnLength;

        private bool _unstableEffect;

        private Brick(Vector3Int position)
        {
            _position = position;
        }

        /// <summary>
        /// //Конструктор позволяет назначить position и pattern блока.
        /// </summary>
        /// <param name="position">Позиция блока</param>
        /// <param name="pattern">Массив кубиков из которых состоит блок</param>
        public Brick(Vector3Int position, Vector3Int[] pattern, int[,] matrix) : this(position)
        {
            _pattern = pattern;
            _matrix = matrix;
            _matrixColumnLength = ComputeColumnLength(matrix.Length);
        }

        public Brick(Vector3Int position, BrickBlank blank) : 
            this(position, blank.Pattern, blank.Matrix)
        {

        }

        private int ComputeColumnLength(int length)
        {
            return (int)Mathf.Sqrt(length);
        }

        /// <summary>
        /// Возвращает копию позиции блока.
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// Возвращает копию паттерна блока.
        /// </summary>
        public Vector3Int[] Pattern => _pattern;
        public int[,] Matrix => _matrix;
        public int MatrixColumnLength => _matrixColumnLength;

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
            int[,] rotatedMatrix = new int[_matrixColumnLength, _matrixColumnLength];

            for (int i = 0; i < _matrixColumnLength; i++)
            {
                for (int j = 0; j < _matrixColumnLength; j++)
                {
                    rotatedMatrix[i, j] = _matrix[_matrixColumnLength - j - 1, i];
                }
            }

            _matrix = rotatedMatrix;

            UpdatePatternByMatrix();
        }

        public void RotateMinus90()
        {
            int[,] rotatedMatrix = new int[_matrixColumnLength, _matrixColumnLength];

            for (int i = 0; i < _matrixColumnLength; i++)
            {
                for (int j = 0; j < _matrixColumnLength; j++)
                {
                    rotatedMatrix[_matrixColumnLength - j - 1, i] = _matrix[i, j];
                }
            }

            _matrix = rotatedMatrix;

            UpdatePatternByMatrix();
        }

        private void UpdatePatternByMatrix()
        {

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