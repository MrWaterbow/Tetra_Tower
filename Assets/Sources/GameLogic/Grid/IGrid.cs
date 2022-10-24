using UnityEngine;

namespace Sources.GridLogic
{
    public interface IGrid
    {
        public Vector2Int Size { get; }

        public Vector3 GetWorldPosition(Vector3 position);
    }
}