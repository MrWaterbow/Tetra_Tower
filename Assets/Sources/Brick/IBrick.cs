using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public interface IReadOnlyBrick
    {
        Vector3Int Position { get; }
        Vector3Int[] Pattern { get; }
    }

    public interface IBrick : IReadOnlyBrick
    {
        void Move(Vector3Int direction);
    }
}