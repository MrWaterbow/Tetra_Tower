using System;
using UnityEngine;

namespace Server.BrickLogic
{

    public sealed class BrickMovementWrapper
    {
        /// <summary>
        /// Метод вызывается, если блок коснулся земли
        /// </summary>
        public event Action OnControllableBrickFall;

        /// <summary>
        /// База данных блоков
        /// </summary>
        private readonly BricksDatabase _database;

        public BrickMovementWrapper(BricksDatabase database)
        {
            _database = database;
        }

        public IReadOnlyBricksDatabase Database => _database;

        /// <summary>
        /// Проверяет возможность движения блока и в случае истины - двигает его в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            if (PossibleMoveBrickTo(direction))
            {
                _database.ControllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// Проверяет возможность движения блока в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _database.Surface.PatternInSurfaceLimits(_database.ControllableBrick.Pattern, featurePosition);
        }

        /// <summary>
        /// Расчитывает будущую позицию блока
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_database.ControllableBrick.Position.x + direction.x, _database.ControllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// Снижает высоту блока на одну единицу и проверяет находится ли он на земле
        /// </summary>
        /// <exception cref="BrickOnGroundException">Исключение выбрасывается в случае того, если блок уже на земле</exception>
        public void LowerBrickAndCheckGrounding()
        {
            if (_database.ControllableBrickOnGround())
            {
                throw new BrickOnGroundException();
            }

            _database.ControllableBrick.Move(Vector3Int.down);

            if (_database.ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();
            }
        }

        /// <summary>
        /// Опускает блок сразу на землю в одно действие
        /// </summary>
        /// <exception cref="BrickOnGroundException">Исключение выбрасывается в случае того, если блок уже на земле</exception>
        public void LowerControllableBrickToGround()
        {
            if (_database.ControllableBrickOnGround())
            {
                throw new BrickOnGroundException();
            }

            int height = _database.GetHeightByPattern(_database.ControllableBrick);
            Vector3Int brickPosition = _database.ControllableBrick.Position;
            Vector3Int newPosition = new(brickPosition.x, height, brickPosition.z);

            _database.ControllableBrick.ChangePosition(newPosition);

            OnControllableBrickFall?.Invoke();
        }
    }
}