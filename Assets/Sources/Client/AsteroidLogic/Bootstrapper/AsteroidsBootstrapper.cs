using Client.BootstrapperLogic;
using Server.AsteroidLogic;
using Server.BrickLogic;
using UnityEngine;
using Zenject;

namespace Client.AsteroidLogic
{
    internal sealed class AsteroidsBootstrapper : Bootstrapper
    {
        [SerializeField] private float _spawnIfCounterValueIs;

        [Space]

        private AsteroidWrapper _asteroidWrapper;
        private IAsteroidViewFactory _factory;
        private IReadOnlyBricksDatabase _database;

        private int _counter;

        [Inject]
        private void Constructor(AsteroidWrapper asteroidWrapper, IAsteroidViewFactory factory, IReadOnlyBricksDatabase database)
        {
            _asteroidWrapper = asteroidWrapper;
            _factory = factory;
            _database = database;
        }

        public override void Boot()
        {
            _database.OnAddBrick += UpdateCounter;
        }

        private void UpdateCounter()
        {
            _counter++;

            if(_counter >= _spawnIfCounterValueIs)
            {
                SpawnAsteroid();

                _counter = 0;
            }
        }

        private void Update()
        {
            _asteroidWrapper.FlyTick();
        }

        private void SpawnAsteroid()
        {
            IReadOnlyAsteroid instance;

            try
            {
                instance = _asteroidWrapper.TryThrowRandomAsteroid();
            }
            catch
            {
                return;
            }

            AsteroidView viewInstance = _factory.Create(instance.Target);

            viewInstance.SetCallbacks(instance, _database.Surface);
        }
    }
}