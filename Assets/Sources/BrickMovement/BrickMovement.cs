using Sources.BricksLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMovement
{
    private readonly BricksSpace _bricksSpace;

    public BrickMovement(BricksSpace bricksSpace)
    {
        _bricksSpace = bricksSpace;
    }

    public void TryMoveBrick(Vector3Int direction)
    {
        Vector2Int featurePosition = ComputeFeaturePosition(direction);

        if (_bricksSpace..PatternInSurfaceLimits(_bricksSpace.ControllableBlockPattern, featurePosition))
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
