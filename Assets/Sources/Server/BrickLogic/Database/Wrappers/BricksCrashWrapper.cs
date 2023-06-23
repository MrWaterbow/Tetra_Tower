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

        public float ComputeFootFactor(IReadOnlyBrick brick)
        {
            float stableTilesCount = GetTilesWhatStabilityFlagEquals(true, brick).Count;
            int tilesCount = brick.Pattern.Length;

            return stableTilesCount / tilesCount;
        }

        public float ComputeDistanceFromNearUnstableTile(Vector3Int stableTilePosition, IReadOnlyBrick brick)
        {
            LinkedList<Vector3Int> unstableTiles = GetTilesWhatStabilityFlagEquals(false, brick);

            return ComputeMinDistance(stableTilePosition, unstableTiles);
        }

        public float ComputeDistanceFromNearStableTile(Vector3Int unstableTilePosition, IReadOnlyBrick brick)
        {
            LinkedList<Vector3Int> stableTiles = GetTilesWhatStabilityFlagEquals(true, brick);

            return ComputeMinDistance(unstableTilePosition, stableTiles);
        }

        public LinkedList<Vector3Int> GetTilesWhatStabilityFlagEquals(bool stable, IReadOnlyBrick brick)
        {
            LinkedList<Vector3Int> tiles = new();

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = brick.Position + tile;
                Vector3Int bricksMapKey = tilePosition - Vector3Int.up;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (IsStableTile(tilePosition, bricksMapKey, heightMapKey) == stable)
                {
                    tiles.AddLast(tilePosition);
                }
            }

            return tiles;
        }

        private bool IsStableTile(Vector3Int tilePosition, Vector3Int bricksMapKey, Vector2Int heightMapKey)
        {
            if (_database.GetBrickByKey(bricksMapKey) != null || tilePosition.y <= 0 && _database.Surface.PositionIntoSurfaceSize(heightMapKey))
            {
                return true;
            }

            return false;
        }

        public float ComputeMinDistance(Vector3Int tilePosition, LinkedList<Vector3Int> tiles)
        {
            float minDistance = ComputeDistance(tilePosition, tiles.First.Value);

            foreach (Vector3Int unstableTile in tiles)
            {
                if (minDistance > ComputeDistance(tilePosition, unstableTile))
                {
                    minDistance = ComputeDistance(tilePosition, unstableTile);
                }
            }

            return minDistance;
        }

        public float ComputeDistance(Vector3Int a, Vector3Int b)
        {
            return Vector3Int.Distance(a, b);
        }
    }
}