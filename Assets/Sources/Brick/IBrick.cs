using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public interface IBrick
    {
        Vector3Int Position { get; }
        Vector3Int[] Pattern { get; }

        void Move(Vector3Int direction);
    }
}