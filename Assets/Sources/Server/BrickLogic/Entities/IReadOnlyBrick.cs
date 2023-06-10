using System;
using UnityEngine;

namespace Server.BrickLogic
{
    public interface IReadOnlyBrick
    {
        event Action<Vector3Int> OnPositionChanged;

        Vector3Int Position { get; }
        Vector3Int[] Pattern { get; }
    }
}