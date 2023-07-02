using Client.BootstrapperLogic;
using Server.AsteroidLogic;
using UnityEngine;
using Zenject;

namespace Client.AsteroidLogic
{
    internal sealed class AsteroidsBootstrapper : Bootstrapper
    {
        [SerializeField] private float _asteroidSpawnRate;

        private AsteroidWrapper _asteroidWrapper;
        private float _asteroidTimer;

        [Inject]
        private void Constructor(AsteroidWrapper asteroidWrapper)
        {
            _asteroidWrapper = asteroidWrapper;
        }

        public override void Boot()
        {

        }

        private void Update()
        {
            _asteroidTimer += Time.deltaTime;

            if (_asteroidTimer >= _asteroidSpawnRate)
            {
                SpawnAsteroid();

                _asteroidTimer = 0f;
            }
        }

        private void SpawnAsteroid()
        {
            _asteroidWrapper.TryThrowRandomAsteroid();
            _asteroidWrapper.AllToTarget();
        }
    }
}