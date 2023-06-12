using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
    {
        /// <summary>
        /// Пространство блоков
        /// </summary>
        private readonly BrickMovementWrapper _brickMovementWrapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">Пространство блоков</param>
        /// <param name="brickInputView">Реализация получения ввода от игрока</param>
        public BrickInputPresenter(BrickMovementWrapper brickMovementWrapper)
        {
            _brickMovementWrapper = brickMovementWrapper;
        }

        /// <summary>
        /// Двигает управляемый блок в указаном направлении
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