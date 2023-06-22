using System;
using UnityEngine;

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

        /// <summary>
        /// //����������� ��������� ��������� position � pattern �����.
        /// </summary>
        /// <param name="position">������� �����</param>
        /// <param name="pattern">������ ������� �� ������� ������� ����</param>
        public Brick(Vector3Int position, Vector3Int[] pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        /// <summary>
        /// ���������� ����� ������� �����.
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// ���������� ����� �������� �����.
        /// </summary>
        public Vector3Int[] Pattern => _pattern;

        /// <summary>
        /// ������� ���� � �������� �����������.
        /// </summary>
        /// <param name="direction">�����������</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;

            OnPositionChanged?.Invoke(_position);
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
    }
}