using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public sealed class BrickMovedEventArgs : EventArgs
    {
        public readonly Brick Brick;

        public BrickMovedEventArgs(Brick brick)
        {
            Brick = brick;
        }
    }

    public sealed class BricksSpace
    {
        //private event EventHandler<BrickMovedEventArgs> _onBlockMoved;

        private readonly List<Brick> _bricks;
        private readonly Brick _controllableBrick;

        private readonly PlacingSurface _surface;

        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, Brick controlledBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<Brick>() { controlledBrick };
            _controllableBrick = controlledBrick;
        }

        public Vector3Int ControllableBlockPosition => _controllableBrick.Position;

        public Vector2Int SurfaceSize => _surface.SurfaceSize;
        public Vector3 WorldPositionOffset => _surface.WorldPositionOffset;

        public void TryMoveBrick(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            if (_surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition))
            {
                _controllableBrick.Move(direction);
            }
        }

        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition);
        }

        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(ControllableBlockPosition.x + direction.x, ControllableBlockPosition.z + direction.z);
        }

        public void LowerControllableBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}