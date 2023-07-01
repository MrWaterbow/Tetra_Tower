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

        public void ThrowRandomAsteroid()
        {

        }

        public void ThrowAsteroid(Vector3Int target)
        {

        }
    }
}