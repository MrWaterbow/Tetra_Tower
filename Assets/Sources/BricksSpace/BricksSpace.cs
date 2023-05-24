using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public sealed class BricksSpace
    {
        private readonly List<IBrick> _bricks;
        private readonly IBrick _controllableBrick;

        private readonly PlacingSurface _surface;

        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, IBrick controlledBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<IBrick>() { controlledBrick };
            _controllableBrick = controlledBrick;
        }

        public Vector3Int ControllableBlockPosition => _controllableBrick.Position;
        public Vector3Int ControllableBlockPattern => _controllableBrick.Position;

        public Vector2Int SurfaceSize => _surface.SurfaceSize;
        public Vector3 WorldPositionOffset => _surface.WorldPositionOffset;
    }
}