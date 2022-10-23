using UnityEngine;

namespace Sources.BlockLogic
{
    public interface IBlock
    {
        Vector3 Position { get; }

        void Fall();
    }
}