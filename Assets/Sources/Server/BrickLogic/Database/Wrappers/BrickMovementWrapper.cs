using System;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Модуль для движения контролирумого блока.
    /// </summary>
    public sealed class BrickMovementWrapper
    {
        /// <summary>
        /// Метод вызывается, если блок коснулся земли.
        /// </summary>
        public event Action OnControllableBrickFall;

        /// <summary>
        /// База данных блоков.
        /// </summary>
        private readonly BricksDatabase _database;

        public BrickMovementWrapper(BricksDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Проверяет возможность движения блока и в случае истины - двигает его в указаном направлении.
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
        /// Проверяет возможность движения блока в указаном направлении.
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector3Int featurePosition = ComputeFeaturePosition(direction);

            bool intoSurfaceLimits = _database.Surface.PatternInSurfaceLimits(_database.ControllableBrick.Pattern, new Vector2Int(featurePosition.x, featurePosition.z));
            bool movedIntoAnother = BrickMovedIntoAnotherBrick(_database.ControllableBrick.Pattern, featurePosition);

            return intoSurfaceLimits && movedIntoAnother == false;
        }

        /// <summary>
        /// Расчитывает будущую позицию блока.
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private Vector3Int ComputeFeaturePosition(Vector3Int direction)
        {
            return _database.ControllableBrick.Position + direction;
        }

        private bool BrickMovedIntoAnotherBrick(Vector3Int[] pattern, Vector3Int position)
        {
            bool movedInto = false;

            foreach (Vector3Int tile in pattern)
            {
                Vector3Int tilePosition = tile + position;

                if (_database.GetBrickByKey(tilePosition) != null)
                {
                    movedInto = true;
                }
            }

            return movedInto;
        }

        public void LowerBrickAndCheckGrounding()
        {
            if (_database.ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();

                return;
            }

            _database.ControllableBrick.Move(Vector3Int.down);

            if (_database.ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();
            }
        }

        public void LowerControllableBrickToGround()
        {
            if (_database.ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();

                return;
            }

            int height = _database.GetHeightByBlock(_database.ControllableBrick);
            Vector3Int brickPosition = _database.ControllableBrick.Position;
            Vector3Int newPosition = new(brickPosition.x, height, brickPosition.z);

            _database.ControllableBrick.ChangePosition(newPosition);

            OnControllableBrickFall?.Invoke();
        }
    }
}