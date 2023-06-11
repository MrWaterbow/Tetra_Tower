using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
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

        public void ToGround()
        {
            _brickSpace.LowerControllableBrickToGround();
        }
    }
}