using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public class BrickInputPresenter
    {
        /// <summary>
        /// ������������ ������
        /// </summary>
        private readonly BricksSpace _brickSpace;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">������������ ������</param>
        /// <param name="brickInputView">���������� ��������� ����� �� ������</param>
        public BrickInputPresenter(BricksSpace brickSpace)
        {
            _brickSpace = brickSpace;
        }

        /// <summary>
        /// ������� ����������� ���� � �������� �����������
        /// </summary>
        /// <param name="direction"></param>
        public void MoveTo(Vector3Int direction)
        {
            _brickSpace.TryMoveBrick(direction);
        }
    }
}