using UnityEngine;

namespace Server.BricksLogic
{
    /// <summary>
    ///��������� ��������� IBrick, �� ��������� ����������� ������ ����������� � ������
    /// </summary>
    public sealed class Brick : IBrick
    {
        /// <summary>
        /// ������� �����
        /// </summary>
        private Vector3Int _position;
        /// <summary>
        /// ������ ������� �� ������� ������� ����
        /// </summary>
        private Vector3Int[] _pattern;

        /// <summary>
        /// //����������� ��������� ��������� position � pattern �����
        /// </summary>
        /// <param name="position">������� �����</param>
        /// <param name="pattern">������ ������� �� ������� ������� ����</param>
        public Brick(Vector3Int position, Vector3Int[] pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        // ��������, ����� �������� �������� ��� ������������� �� ���, �.�. � ������ �������/������� � ��.
        /// <summary>
        /// ������ � ������ ������� �����
        /// </summary>
        public Vector3Int Position => _position;
        /// <summary>
        /// ������ � ������ ������� �������, �� ������� ������� ����
        /// </summary>
        public Vector3Int[] Pattern => _pattern;

        /// <summary>
        /// �������������� ������� �������, ��� �� ������� ������� �������� � ��������� �����
        /// </summary>
        /// <param name="direction">�����������</param>
        public void Move(Vector3Int direction)
        {
            _position += direction;
        }
    }
}