using UnityEngine;

namespace Client.AsteroidLogic
{
    internal sealed class AsteroidViewFactory : IAsteroidViewFactory
    {
        private readonly AsteroidView _prefab;
        private readonly Transform _spawnPoint;

        public AsteroidViewFactory(AsteroidView prefab, Transform spawnPoint)
        {
            _prefab = prefab;
            _spawnPoint = spawnPoint;
        }

        public AsteroidView Create(Vector3Int target)
        {
            AsteroidView instance = Object.Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);

            instance.Initialize(target);

            return instance;
        }
    }
}