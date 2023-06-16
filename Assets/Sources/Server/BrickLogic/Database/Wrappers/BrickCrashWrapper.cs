using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BrickCrashWrapper
    {
        private readonly BricksDatabase _database;

        public BrickCrashWrapper(BricksDatabase database)
        {
            _database = database;
        }

        public void CheckBricksCrashing()
        {

        }

        public float ComputeFootFactor(Brick brick)
        {
            float stableTilesCount = 0;

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = brick.Position + tile;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if(_database.GetHeightByKey(heightMapKey) == tilePosition.y && _database.Surface.PositionInSurfaceLimits(heightMapKey))
                {
                    stableTilesCount++;
                }
            }

            return stableTilesCount / brick.Pattern.Length;
        }
    }
}