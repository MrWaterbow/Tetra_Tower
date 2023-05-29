using Server.BricksLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Database.BricksLogic
{
    public interface IReadOnlyBricksSpaceDatabase
    {
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }

        IReadOnlyBrick ControllableBrick { get; }

        Vector3Int ComputeFeatureGroundPosition(Vector3Int direction);
    }
}