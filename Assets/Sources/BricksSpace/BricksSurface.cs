using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public readonly struct BricksSurface
    {
        public readonly Vector2Int SurfaceSize;
        public readonly Vector3 WorldPositionOffset;

        public BricksSurface(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            SurfaceSize = surfaceSize;
            WorldPositionOffset = worldPositionOffset;
        }

        public bool PatternInSurfaceLimits(IReadOnlyCollection<Vector3Int> pattern, Vector2Int position)
        {
            foreach (Vector3Int cell in pattern)
            {
                Vector2Int featureCellPosition = new Vector2Int(cell.x, cell.z) + position;

                if (PositionInSurfaceLimits(featureCellPosition)) return true;
            }

            return false;
        }

        public bool PositionInSurfaceLimits(Vector2Int position)
        {
            return position.x > -1 && position.x < SurfaceSize.x && position.y > -1 && position.y < SurfaceSize.y;
        }
    }
}