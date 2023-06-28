using UnityEngine;

namespace Server.BrickLogic
{
    public readonly struct MatrixTile
    {
        public readonly Vector2Int OffsetPosition;
        public readonly bool IsTile;

        public MatrixTile(Vector2Int offsetPosition, bool isTile)
        {
            OffsetPosition = offsetPosition;
            IsTile = isTile;
        }
    }
}