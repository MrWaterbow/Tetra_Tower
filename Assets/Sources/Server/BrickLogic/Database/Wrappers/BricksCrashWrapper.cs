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
            LinkedList<IReadOnlyBrick> destroyingBricks = new();

            foreach (Brick brick in _database.Bricks)
            {
                if(TestToCrash(brick, destroyingBricks))
                {
                    destroyingBricks.AddLast(brick);
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

        public bool TestToCrash(Brick brick, LinkedList<IReadOnlyBrick> destroyingBricks)
        {
            float footFactor = ComputeFootFactor(brick, destroyingBricks);

            if(footFactor < 0)
            {
                return true;
            }
            if(footFactor == 0)
            {
                brick.InvokeUnstableWarning(true);
            }
            if(footFactor > 0)
            {
                brick.InvokeUnstableWarning(false);
            }

            return false;
        }

        public float ComputeFootFactor(IReadOnlyBrick brick, LinkedList<IReadOnlyBrick> destroyingBricks = null)
        {
            if(destroyingBricks == null)
            {
                destroyingBricks = new();
            }

            LinkedList<Vector3Int> stableTiles = GetTilesWhatStabilityFlagEquals(true, brick, destroyingBricks);
            LinkedList<Vector3Int> unstableTiles = GetTilesWhatStabilityFlagEquals(false, brick, destroyingBricks);
            float stableScore = 0f;
            float unstableScore = 0f;

            foreach (Vector3Int stableTile in stableTiles)
            {
                stableScore += ComputeDistanceFromNearUnstableTile(stableTile, brick, destroyingBricks);
            }

            foreach (Vector3Int unstableTile in unstableTiles)
            {
                unstableScore += ComputeDistanceFromNearStableTile(unstableTile, brick, destroyingBricks);
            }

            return stableScore - unstableScore;
        }

        public float ComputeDistanceFromNearUnstableTile(Vector3Int stableTilePosition, IReadOnlyBrick brick, LinkedList<IReadOnlyBrick> destroyingBricks = null)
        {
            if(destroyingBricks == null)
            {
                destroyingBricks = new();
            }

            LinkedList<Vector3Int> unstableTiles = GetTilesWhatStabilityFlagEquals(false, brick, destroyingBricks);

            return ComputeMinDistance(stableTilePosition, unstableTiles);
        }

        public float ComputeDistanceFromNearStableTile(Vector3Int unstableTilePosition, IReadOnlyBrick brick, LinkedList<IReadOnlyBrick> destroyingBricks = null)
        {
            if (destroyingBricks == null)
            {
                destroyingBricks = new();
            }

            LinkedList<Vector3Int> stableTiles = GetTilesWhatStabilityFlagEquals(true, brick, destroyingBricks);

            return ComputeMinDistance(unstableTilePosition, stableTiles);
        }

        public LinkedList<Vector3Int> GetTilesWhatStabilityFlagEquals(bool stable, IReadOnlyBrick brick, LinkedList<IReadOnlyBrick> destroyingBricks)
        {
            LinkedList<IReadOnlyBrick> bricks = new();
            LinkedList<Vector3Int> tiles = new();

            bricks.AddLast(brick);

            LinkedList<IReadOnlyBrick> upperBricks = _database.GetOnlyFirstUpperBricks(brick);

            if (stable)
            {
                foreach (IReadOnlyBrick upperBrick in upperBricks)
                {
                    if (bricks.Contains(upperBrick) || ProductiveComputeFootFactor(upperBrick) <= 0.5f) continue;

                    bricks.AddLast(upperBrick);
                }
            }
            else
            {
                foreach (IReadOnlyBrick upperBrick in upperBricks)
                {
                    if (bricks.Contains(upperBrick) || IsNegativeSupportBrick(upperBrick, brick) == false) continue;

                    bricks.AddLast(upperBrick);
                }
            }

            foreach (IReadOnlyBrick processBrick in bricks)
            {
                foreach (Vector3Int tile in processBrick.Pattern)
                {
                    Vector3Int tilePosition = processBrick.Position + tile;
                    Vector3Int bricksMapKey = tilePosition - Vector3Int.up;
                    Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                    if (IsStableTile(bricksMapKey, heightMapKey) == stable && destroyingBricks.Contains(_database.GetBrickByKey(bricksMapKey)) == false)
                    {
                        tiles.AddLast(tilePosition);
                    }
                }
            }

            return tiles;
        }

        private float ProductiveComputeFootFactor(IReadOnlyBrick brick)
        {
            int unstableTilesCount = 0;

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = tile + brick.Position;
                Vector3Int underTilePosition = tilePosition - Vector3Int.up;
                Vector2Int keyPosition = new(tilePosition.x, tilePosition.z);

                if(IsStableTile(underTilePosition, keyPosition) == false)
                {
                    unstableTilesCount++;
                }
            }

            return unstableTilesCount / brick.Pattern.Length;
        }

        private bool IsNegativeSupportBrick(IReadOnlyBrick upperBrick, IReadOnlyBrick startBrick)
        {
            foreach (Vector3Int tile in startBrick.Pattern)
            {
                Vector3Int tilePosition = tile + startBrick.Position;
                Vector2Int tileKeyPosition = new(tilePosition.x, tilePosition.z);

                foreach (Vector3Int upperTile in upperBrick.Pattern)
                {
                    Vector3Int upperTilePosition = upperTile + upperBrick.Position;
                    Vector2Int upperTileKeyPosition = new(upperTilePosition.x, upperTilePosition.z);

                    if (tileKeyPosition == upperTileKeyPosition && IsStableTile(tilePosition - Vector3Int.up, tileKeyPosition) == false)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsStableTile(Vector3Int bricksMapKey, Vector2Int heightMapKey)
        {
            if (_database.GetBrickByKey(bricksMapKey) != null || bricksMapKey.y < 0 && _database.Surface.PositionIntoSurfaceSize(heightMapKey) && _database.Surface.PositionIntoSurfaceTiles(heightMapKey))
            {
                return true;
            }

            return false;
        }

        public float ComputeMinDistance(Vector3Int tilePosition, LinkedList<Vector3Int> tiles)
        {
            if (tiles.Count == 0) return 1f;

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