using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksCrashWrapper
    {
        private readonly BricksDatabase _database;

        public BricksCrashWrapper(BricksDatabase database)
        {
            _database = database;
        }

        public void TryCrashAll()
        {
            List<Brick> destroyingBricks = new();

            foreach (Brick brick in _database.Bricks)
            {
                if(TestToCrash(brick))
                {
                    destroyingBricks.Add(brick);
                }
            }

            foreach (Brick destroyingBrick in destroyingBricks)
            {
                _database.DestroyBrick(destroyingBrick);
            }
        }

        public void CrashAll()
        {
            List<Brick> destroyingBricks = new();

            foreach (Brick brick in _database.Bricks)
            {
                destroyingBricks.Add(brick);
            }

            foreach (Brick destroyingBrick in destroyingBricks)
            {
                _database.DestroyBrick(destroyingBrick);
            }
        }

        public bool TestToCrash(Brick brick)
        {
            float footFactor = ComputeFootFactor(brick);

            if(footFactor < 0.5f)
            {
                return true;
            }
            if(footFactor == 0.5f)
            {
                brick.InvokeUnstableWarning(true);
            }

            return false;
        }

        public float ComputeFootFactor(Brick brick)
        {
            float stableTilesCount = 0;

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = brick.Position + tile;
                Vector3Int bricksMapKey = tilePosition - Vector3Int.up;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (_database.GetBrickByKey(bricksMapKey) != null || tilePosition.y <= 0 && _database.Surface.PositionInSurfaceLimits(heightMapKey))
                {
                    stableTilesCount++;
                }
            }

            return stableTilesCount / brick.Pattern.Length;
        }
    }
}