using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Server.BrickLogic
{
    /// <summary>
    /// �����, ������� ������������ �������� �����.
    /// </summary>
    public class Brick : IReadOnlyBrick
    {
        /// <summary>
        /// ����� ���������� ��� ����� ������� �����.
        /// </summary>
        public event Action<Vector3Int> OnPositionChanged;
        public event Action<Vector3Int[]> OnRotate90;

        public event Action<bool> UnstableWarning;
        public event Action OnDestroy;

        /// <summary>
        /// ������� �����.
        /// </summary>
        private Vector3Int _position;
        /// <summary>
        /// ������ ������� �� ������� ������� ����.
        /// </summary>
        private Vector3Int[] _pattern;
        private int[,] _matrix;
        private readonly int _matrixColumnLength;
        private readonly Vector2Int _center;

        private bool _unstableEffect;

        private Brick(Vector3Int position)
        {
            _position = position;
        }

        /// <summary>
        /// //����������� ��������� ��������� position � pattern �����.
        /// </summary>
        /// <param name="position">������� �����</param>
        /// <param name="pattern">������ ������� �� ������� ������� ����</param>
        public Brick(Vector3Int position, Vector3Int[] pattern, int[,] matrix, Vector2Int center) : this(position)
        {
            _pattern = pattern;
            _matrix = matrix;
            _matrixColumnLength = ComputeColumnLength(matrix.Length);
            _center = center;
        }

        public Brick(Vector3Int position, BrickBlank blank) : 
            this(position, blank.Pattern, blank.Matrix, blank.Center)
        {

        }

        private int ComputeColumnLength(int length)
        {
            return (int)Mathf.Sqrt(length);
        }

        /// <summary>
        /// ���������� ����� ������� �����.
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// ���������� ����� �������� �����.
        /// </summary>
        public Vector3Int[] Pattern => _pattern;
        public int[,] Matrix => _matrix;
        public int MatrixColumnLength => _matrixColumnLength;

        public bool UnstableEffect => _unstableEffect;

        /// <summary>
        /// ������� ���� � �������� �����������.
        /// </summary>
        /// <param name="direction">�����������</param>
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

            OnRotate90?.Invoke(_pattern);
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
            _pattern = GetPatternByMatrix(GetIndicesOfMatrix()).ToArray();
        }

        public LinkedList<Vector3Int> GetPatternByMatrix(LinkedList<Vector2Int> rawIndices)
        {
            LinkedList<Vector3Int> directions = new();

            foreach (Vector2Int index in rawIndices)
            {
                // ( 0, 1 )
                // ( 0, 1 ) - ( 1, 1 ) = ( -1, 0 )

                //Vector2Int indexDirection = index - _center;
                Vector3Int direction = new(index.x, 0, index.y);

                //if(direction.x != 0)
                //{
                //    direction.y = 1;
                //}

                directions.AddLast(direction);
            }

            return directions;
        }

        public LinkedList<Vector2Int> GetIndicesOfMatrix()
        {
            LinkedList<Vector2Int> result = new();

            for (int i = 0; i < _matrixColumnLength; i++)
            {
                for (int j = 0; j < _matrixColumnLength; j++)
                {
                    if (_matrix[j, i] == 1)
                    {
                        result.AddLast(new Vector2Int(j, i));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// ������ ������� �����.
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