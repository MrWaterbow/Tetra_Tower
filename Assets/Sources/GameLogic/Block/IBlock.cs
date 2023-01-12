using Sources.StateMachines;
using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        event Action<Vector3> Transforming;

        Vector3Int[] Size { get; }
        Vector3 VisualizationOffset { get; }

        Vector3Int Position { get; }

        IReadonlyStateMachine<BlockState> StateMachine { get; }

        bool Instable { get; }

        Transform Transform { get; }
        Transform ModelTransform { get; }
        MeshRenderer MeshRenderer { get; }
        MeshFilter MeshFilter { get; }

        void Move(Vector3Int direction);
        void SetCenterOfMass(Vector3 offset);

        void Fall();
        void Destroy();

        void InvokeInstable();
        void DeinvokeInstable();
        void InvokeRigidbody();

        void Rotate(Vector3 direction, int degree);
    }
}