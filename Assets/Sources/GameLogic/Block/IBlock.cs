using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        event Action<Vector3> Moved;
        event Action Placed;

        Transform OffsetTransform { get; }
        MeshRenderer MeshRenderer { get; }
        MeshFilter MeshFilter { get; }

        Vector3 Position { get; }

        void Move(Vector3 direction);

        void Fall();
    }
}