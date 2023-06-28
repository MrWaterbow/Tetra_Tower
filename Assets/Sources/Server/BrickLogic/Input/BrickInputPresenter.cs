using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
    {
        /// <summary>
        /// ������������ ������.
        /// </summary>
        private readonly BrickMovementWrapper _movementWrapper;
        private readonly BricksRotatingWrapper _rotatingWrapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">������������ ������</param>
        /// <param name="brickInputView">���������� ��������� ����� �� ������</param>
        public BrickInputPresenter(BrickMovementWrapper movementWrapper, BricksRotatingWrapper rotatingWrapper)
        {
            _movementWrapper = movementWrapper;
            _rotatingWrapper = rotatingWrapper;
        }

        /// <summary>
        /// ������� ����������� ���� � �������� �����������.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveTo(Vector3Int direction)
        {
            _movementWrapper.TryMoveBrick(direction);
        }

        public void Rotate()
        {
            _rotatingWrapper.TryRotate90();
        }

        /// <summary>
        /// ����������� �������� ���� �� �����.
        /// </summary>
        public void ToGround()
        {
            _movementWrapper.LowerControllableBrickToGround();
        }
    }
}