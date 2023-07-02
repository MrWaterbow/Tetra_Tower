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

        public void TryThrowRandomAsteroid()
        {
            try
            {
                Vector3Int target = _database.TryGetRandomBricksMapKey();

                ThrowAsteroid(target);
            }
            catch
            { }
        }

        public void ThrowAsteroid(Vector3Int target)
        {
            _database.AddAsteroid(target);
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