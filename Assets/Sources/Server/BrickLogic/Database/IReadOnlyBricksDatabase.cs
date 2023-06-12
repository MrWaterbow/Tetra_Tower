using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public interface IReadOnlyBricksDatabase
    {
        IReadOnlyDictionary<Vector2Int, int> HeightMap { get; }
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }
        IReadOnlyBrick ControllableBrick { get; }
        PlacingSurface Surface { get; }
    }
}