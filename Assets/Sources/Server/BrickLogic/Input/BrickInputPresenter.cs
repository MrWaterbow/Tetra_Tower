using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
    {
        /// <summary>
        /// Пространство блоков.
        /// </summary>
        private readonly BrickMovementWrapper _movementWrapper;
        private readonly BricksRotatingWrapper _rotatingWrapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">Пространство блоков</param>
        /// <param name="brickInputView">Реализация получения ввода от игрока</param>
        public BrickInputPresenter(BrickMovementWrapper movementWrapper, BricksRotatingWrapper rotatingWrapper)
        {
            _movementWrapper = movementWrapper;
            _rotatingWrapper = rotatingWrapper;
        }

        /// <summary>
        /// Двигает управляемый блок в указаном направлении.
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
        /// Моментально опускает блок на землю.
        /// </summary>
        public void ToGround()
        {
            _movementWrapper.LowerControllableBrickToGround();
        }
    }
}