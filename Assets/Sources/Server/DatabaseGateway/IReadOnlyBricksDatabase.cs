using System.Collections.Generic;
using UnityEngine;

namespace Server.Database
{
    public interface IReadOnlyBricksDatabase
    {
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }

        IReadOnlyBrick ControllableBrick { get; }

        Vector3Int ComputeFeatureGroundPosition(Vector3Int direction);
    }
}