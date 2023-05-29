using System;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IReadOnlyBrick
    {
        event Action<Vector3Int> OnPositionChanged;

        public Vector3Int Position { get; }
        public Vector3Int[] Pattern { get; }
    }
}