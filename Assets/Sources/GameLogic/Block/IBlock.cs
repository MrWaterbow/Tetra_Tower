using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        event Action<Vector3> Moved;
        event Action Placed;

        Vector3Int[] Size { get; }
        Vector3 VisualizationOffset { get; }

        Vector3Int Position { get; }

        bool Instable { get; }

        Transform Transform { get; }
        Transform ModelTransform { get; }
        MeshRenderer MeshRenderer { get; }
        MeshFilter MeshFilter { get; }

        void Move(Vector3Int direction);

        void Fall();
        void Destroy();

        void MakeInstable(); 
        void ActivePhysics();

        void Rotate(Vector3 direction, int degree);

        Vector3 GetRaycast();
    }
}