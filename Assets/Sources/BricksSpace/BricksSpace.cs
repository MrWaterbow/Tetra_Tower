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

        public IReadOnlyBrick ControllableBrick => _controllableBrick;

        public PlacingSurface Surface => _surface;

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
            return new(_controllableBrick.Position.x + direction.x, _controllableBrick.Position.z + direction.z);
        }

        public void LowerControllableBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}