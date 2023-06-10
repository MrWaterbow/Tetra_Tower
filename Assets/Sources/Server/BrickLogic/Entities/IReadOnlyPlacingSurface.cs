using UnityEngine;

namespace Server.BrickLogic
{
    public interface IReadOnlyPlacingSurface
    {
        Vector2Int SurfaceSize { get; }
        Vector3 WorldPositionOffset { get; }

        Vector3 GetWorldPosition(Vector3Int localPosition);
    }
}