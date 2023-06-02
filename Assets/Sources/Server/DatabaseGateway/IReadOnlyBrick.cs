using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.Database
{
    public interface IReadOnlyBrick
    {
        event Action<Vector3Int> OnPositionChanged;

        Vector3Int Position { get; }
        IReadOnlyCollection<Vector3Int> Pattern { get; }
    }
}