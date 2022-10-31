using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        public event Action<Vector3> Moved;

        Vector3 Position { get; }

        void Move(Vector3 direction);

        void Fall();
    }
}