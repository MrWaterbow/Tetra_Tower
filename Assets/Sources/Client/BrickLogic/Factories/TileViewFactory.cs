using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class TileViewFactory : ITileViewFactory
    {
        private readonly BrickTileView _prefab;
        private readonly Transform _parent;

        public TileViewFactory(BrickTileView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public BrickTileView Create(Vector3Int position)
        {
            Vector3 meshSize = _prefab.Mesh.bounds.size;
            Vector3 worldPosition = new(position.x * meshSize.x, position.y * meshSize.y, position.z * meshSize.z);

            BrickTileView instance = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parent);
            instance.transform.localPosition = worldPosition;

            return instance;
        }
    }
}