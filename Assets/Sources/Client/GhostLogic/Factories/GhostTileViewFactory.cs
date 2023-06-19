using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostTileViewFactory : IGhostTileViewFactory
    {
        private readonly GhostTileView _prefab;
        private readonly Transform _parent;

        public GhostTileViewFactory(GhostTileView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GhostTileView Create(Vector3Int position)
        {
            Vector3 meshSize = _prefab.Mesh.bounds.size;
            Vector3 worldPosition = new(position.x * meshSize.x, position.y * meshSize.y, position.z * meshSize.z);

            GhostTileView instance = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent);
            instance.transform.localPosition = worldPosition;

            return instance;
        }
    }
}