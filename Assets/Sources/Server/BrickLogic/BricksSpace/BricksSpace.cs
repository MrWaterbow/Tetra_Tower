using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BricksSpace
    {
        /// <summary>
        /// Метод вызывается, если блок коснулся земли
        /// </summary>
        public event Action OnControllableBrickFall;

        /// <summary>
        /// База данных блоков
        /// </summary>
        public BricksSpaceDatabase Database;

        /// <param name="surfaceSize">Размер платформы</param>
        /// <param name="worldPositionOffset">Смещение относительно мировых координат</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            Database = new(surfaceSize, worldPositionOffset);
        }

        /// <param name="placingSurface">Платформа на которую ставяться блоки</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(PlacingSurface placingSurface)
        {
            Database = new(placingSurface);
        }

        /// <summary>
        /// Проверяет возможность движения блока и в случае истины - двигает его в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            if (Database.PossibleMoveBrickTo(direction))
            {
                Database.ControllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// Снижает высоту блока на одну единицу и проверяет находится ли он на земле
        /// </summary>
        /// <exception cref="BrickOnGroundException">Исключение выбрасывается в случае того, если блок уже на земле</exception>
        public void LowerBrickAndCheckGrounding()
        {
            if (Database.ControllableBrickOnGround() == false)
            {
                Database.ControllableBrick.Move(Vector3Int.down);
            }
            else
            {
                throw new BrickOnGroundException();
            }

            if (Database.ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();
            }
        }

        /// <summary>
        /// Опускает блок сразу на землю в одно действие
        /// </summary>
        /// <exception cref="BrickOnGroundException">Исключение выбрасывается в случае того, если блок уже на земле</exception>
        public void LowerBrickToGround()
        {
            if(Database.ControllableBrickOnGround())
            {
                throw new BrickOnGroundException();
            }

            Vector3Int direction = Vector3Int.down * Database.ControllableBrick.Position.y;

            Database.ControllableBrick.Move(direction);

            OnControllableBrickFall?.Invoke();
        }

        /// <summary>
        /// Меняет управляемый игроком блок и добавляет новый в список блоков
        /// </summary>
        /// <param name="brick"></param>
        public void ChangeAndAddRecentControllableBrick(Brick brick)
        {
            Database.ControllableBrick = brick;
            Database.Bricks.Add(brick);
        }
    }
}