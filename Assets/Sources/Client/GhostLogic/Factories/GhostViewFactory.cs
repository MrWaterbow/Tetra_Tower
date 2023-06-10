using UnityEngine;

namespace Client.GhostLogic
{
    internal sealed class GhostViewFactory : IGhostViewFactory
    {
        private readonly GhostView _prefab;

        public GhostViewFactory(GhostView prefab)
        {
            _prefab = prefab;
        }

        public GhostView Create()
        {
            return Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        }
    }
}