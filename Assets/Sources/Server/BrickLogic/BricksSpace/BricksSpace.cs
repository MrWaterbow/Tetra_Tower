using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
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
        private BricksDatabase _database;

        /// <param name="surfaceSize">Размер платформы</param>
        /// <param name="worldPositionOffset">Смещение относительно мировых координат</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            _database = new(surfaceSize, worldPositionOffset);
        }

        /// <param name="placingSurface">Платформа на которую ставяться блоки</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(PlacingSurface placingSurface)
        {
            _database = new(placingSurface);
        }

        public IReadOnlyList<IReadOnlyBrick> Bricks => _database.Bricks;
        public IReadOnlyBrick ControllableBrick => _database.ControllableBrick;

        public PlacingSurface Surface => _database.Surface;

        /// <summary>
        /// Проверяет возможность движения блока и в случае истины - двигает его в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            if (_database.PossibleMoveBrickTo(direction))
            {
                _database.ControllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// Снижает высоту блока на одну единицу и проверяет находится ли он на земле
        /// </summary>
        /// <exception cref="BrickOnGroundException">Исключение выбрасывается в случае того, если блок уже на земле</exception>
        public void LowerBrickAndCheckGrounding()
        {
            if (_database.ControllableBrickOnGround() == false)
            {
                _database.ControllableBrick.Move(Vector3Int.down);
            }
            else
            {
                throw new BrickOnGroundException();
            }

            if (_database.ControllableBrickOnGround())
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
            if (_database.ControllableBrickOnGround())
            {
                throw new BrickOnGroundException();
            }

            Vector3Int direction = Vector3Int.down * _database.ControllableBrick.Position.y;

            _database.ControllableBrick.Move(direction);

            OnControllableBrickFall?.Invoke();
        }

        /// <summary>
        /// Меняет управляемый игроком блок и добавляет новый в список блоков
        /// </summary>
        /// <param name="brick"></param>
        public void ChangeAndAddRecentControllableBrick(Brick brick)
        {
            _database.ControllableBrick = brick;
            _database.Bricks.Add(brick);
        }

        public Vector3Int ComputeFeatureGroundPosition(Vector3Int position)
        {
            return position * new Vector3Int(1, 0, 1);
        }
    }
}