using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksRotatingWrapper
    {
        private readonly BricksDatabase _database;

        public BricksRotatingWrapper(BricksDatabase database)
        {
            _database = database;
        }

        public void TryRotate90()
        {
            if (PossibleRotateBrick() == false) return;

            _database.ControllableBrick.Rotate90();
        }

        public bool PossibleRotateBrick()
        {
            Vector3Int[] featurePattern = _database.ControllableBrick.GetFeatureRotatePattern();
            Vector3Int position = _database.ControllableBrick.Position;

            bool intoSurfaceLimits = _database.Surface.PatternIntoSurfaceTiles(featurePattern, new(position.x, position.z));
            bool rotatedIntoAnother = RotatedIntoAnotherBrick(featurePattern, position);

            return intoSurfaceLimits && rotatedIntoAnother == false;
        }

        private bool RotatedIntoAnotherBrick(Vector3Int[] pattern, Vector3Int position)
        {
            bool rotatedInto = false;

            foreach (Vector3Int tile in pattern)
            {
                Vector3Int tilePosition = tile + position;

                if(_database.GetBrickByKey(tilePosition) != null)
                {
                    rotatedInto = true;
                }
            }

            return rotatedInto;
        }

        /// <summary>
        /// Проверяет возможность поворота блока на 90 градусов.
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        //private bool PossibleMoveBrickTo(Vector3Int direction)
        //{
        //    Vector3Int[] featurePattern = Compute(direction);

        //    bool intoSurfaceLimits = _bricksDatabase.Surface.PatternIntoSurfaceTiles(_bricksDatabase.ControllableBrick.Pattern, new Vector2Int(featurePosition.x, featurePosition.z));
        //    bool movedIntoAnother = BrickMovedIntoAnotherBrick(_bricksDatabase.ControllableBrick.Pattern, featurePosition);

        //    return intoSurfaceLimits && movedIntoAnother == false;
        //}
    }
}