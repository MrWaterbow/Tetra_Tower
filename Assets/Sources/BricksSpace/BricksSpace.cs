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

        private readonly BricksSurface _surface;

        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, List<Brick> bricks, Brick controlledBrick)
        {
            _surface = new BricksSurface(surfaceSize, worldPositionOffset);

            _bricks = bricks;
            _controlledBrick = controlledBrick;
        }

        public Vector3Int ControlledBlockPosition => _controlledBrick.Position;

        public Vector2Int SurfaceSize => _surface.SurfaceSize;
        public Vector3 WorldPositionOffset => _surface.WorldPositionOffset;

        //private Vector2Int ControlledBlockPositionInto2D => new Vector2Int(ControlledBlockPosition.x, ControlledBlockPosition.z);

        public void TryMoveControlledBrick(Vector3Int direction)
        {
            Vector2Int featurePosition = new(ControlledBlockPosition.x + direction.x, ControlledBlockPosition.z + direction.z);

            if(_surface.PatternInSurfaceLimits(_controlledBrick.Pattern, featurePosition))
            {
                _controlledBrick.Move(direction);
            }
        }

        public bool PossibleToMove(Vector3Int direction)
        {
            Vector2Int featurePosition = new(ControlledBlockPosition.x + direction.x, ControlledBlockPosition.z + direction.z);

            return _surface.PatternInSurfaceLimits(_controlledBrick.Pattern, featurePosition);
        }

        public void LowerControlledBrick()
        {
            _controlledBrick.Move(Vector3Int.down);
        }
    }
}