using System;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksDatabaseAccess
    {
        private readonly BricksDatabase _database;

        public BricksDatabaseAccess(BricksDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Меняет управляемый игроком блок и добавляет новый в список блоков
        /// </summary>
        /// <param name="brick"></param>
        public void ChangeAndAddRecentControllableBrick(Brick brick)
        {
            if (_database.ControllableBrick != null)
            {
                _database.AddBrickAndUpdateHeightMap(_database.ControllableBrick);
            }

            _database.ControllableBrick = brick;
        }

        public void PlaceControllableBrick()
        {
            _database.AddBrickAndUpdateHeightMap(_database.ControllableBrick);

            _database.ControllableBrick = null;
        }
    }

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
            if (ControllableBrickOnGround() == false)
            {
                _database.ControllableBrick.Move(Vector3Int.down);
            }
            else
            {
                throw new BrickOnGroundException();
            }

            if (ControllableBrickOnGround())
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
            if (ControllableBrickOnGround())
            {
                throw new BrickOnGroundException();
            }

            int height = GetHeightByPattern(_database.ControllableBrick);
            Vector3Int brickPosition = _database.ControllableBrick.Position;
            Vector3Int newPosition = new(brickPosition.x, height, brickPosition.z);

            _database.ControllableBrick.ChangePosition(newPosition);

            OnControllableBrickFall?.Invoke();
        }

        /// <summary>
        /// Проверка блока находится ли он на земле
        /// </summary>
        /// <returns></returns>
        private bool ControllableBrickOnGround()
        {
            bool onGround = false;

            foreach (Vector3Int patternTile in _database.ControllableBrick.Pattern)
            {
                Vector3Int tilePosition = patternTile + _database.ControllableBrick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (_database.HeightMap[heightMapKey] == tilePosition.y - 1)
                {
                    onGround = true;
                }
            }

            return onGround;
        }

        private int GetHeightByPattern(Brick brick)
        {
            int height = 0;

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (_database.HeightMap[heightMapKey] > height)
                {
                    height = _database.HeightMap[heightMapKey] + 1;
                }
            }

            return height;
        }

        public Vector3 GetControllableBrickWorldPosition()
        {
            return _database.Surface.GetWorldPosition(_database.ControllableBrick.Position);
        }
    }
}