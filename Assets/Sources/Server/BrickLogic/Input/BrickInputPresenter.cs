using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickInputPresenter : IBrickInputPresenter
    {
        /// <summary>
        /// Пространство блоков
        /// </summary>
        private readonly BricksSpace _brickSpace;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">Пространство блоков</param>
        /// <param name="brickInputView">Реализация получения ввода от игрока</param>
        public BrickInputPresenter(BricksSpace brickSpace)
        {
            _brickSpace = brickSpace;
        }

        /// <summary>
        /// Двигает управляемый блок в указаном направлении
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