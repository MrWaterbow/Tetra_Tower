using System;
using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class AsteroidWrapper
    {
        private readonly AsteroidsDatabase _database;

        public AsteroidWrapper(AsteroidsDatabase database)
        {
            _database = database;
        }

        public IReadOnlyAsteroid TryThrowRandomAsteroid()
        {
            try
            {
                Vector3Int target = _database.TryGetRandomBricksMapKey();

                return ThrowAsteroid(target);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            throw new NullReferenceException("Asteroid creating error");
        }

        public IReadOnlyAsteroid ThrowAsteroid(Vector3Int target)
        {
            return _database.AddAsteroid(target);
        }

        public void FlyTick()
        {
            _database.FlyTick();
        }

        public void AllToTarget()
        {
            _database.AllToTargets();
        }
    }
}