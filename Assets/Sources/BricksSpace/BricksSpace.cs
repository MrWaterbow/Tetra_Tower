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
        private readonly Brick _controlledBrick;

        private readonly PlacingSurface _surface;

        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, Brick controlledBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<Brick>() { controlledBrick };
            _controlledBrick = controlledBrick;
        }

        public Vector3Int ControlledBlockPosition => _controlledBrick.Position;

        public Vector2Int SurfaceSize => _surface.SurfaceSize;
        public Vector3 WorldPositionOffset => _surface.WorldPositionOffset;

        public void TryMoveBrick(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            if (_surface.PatternInSurfaceLimits(_controlledBrick.Pattern, featurePosition))
            {
                _controlledBrick.Move(direction);
            }
        }

        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _surface.PatternInSurfaceLimits(_controlledBrick.Pattern, featurePosition);
        }

        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(ControlledBlockPosition.x + direction.x, ControlledBlockPosition.z + direction.z);
        }

        public void LowerControlledBrick()
        {
            _controlledBrick.Move(Vector3Int.down);
        }
    }
}