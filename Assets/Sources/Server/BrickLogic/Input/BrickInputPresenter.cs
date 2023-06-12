using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
    {
        /// <summary>
        /// ������������ ������
        /// </summary>
        private readonly BrickMovementWrapper _brickMovementWrapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">������������ ������</param>
        /// <param name="brickInputView">���������� ��������� ����� �� ������</param>
        public BrickInputPresenter(BrickMovementWrapper brickMovementWrapper)
        {
            _brickMovementWrapper = brickMovementWrapper;
        }

        /// <summary>
        /// ������� ����������� ���� � �������� �����������
        /// </summary>
        /// <param name="direction"></param>
        public void MoveTo(Vector3Int direction)
        {
            _brickMovementWrapper.TryMoveBrick(direction);
        }

        public void ToGround()
        {
            _brickMovementWrapper.LowerControllableBrickToGround();
        }
    }
}