using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        event Action<Vector3> Moved;
        event Action Placed;

        Vector3Int[] Size { get; }

        Vector3 Position { get; }

        Transform Transform { get; }
        MeshRenderer MeshRenderer { get; }
        MeshFilter MeshFilter { get; }

        void Move(Vector3 direction);

        void Fall();

        void Rotate(Vector3 direction, int degree);
    }
}