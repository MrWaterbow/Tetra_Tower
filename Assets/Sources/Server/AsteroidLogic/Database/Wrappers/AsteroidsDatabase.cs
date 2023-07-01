using Server.BrickLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class AsteroidsDatabase
    {
        private readonly BricksDatabase _bricksDatabase;

        private readonly LinkedList<Asteroid> _asteroids;
        private readonly IAsteroidFactory _factory;

        public AsteroidsDatabase(BricksDatabase database, IAsteroidFactory factory)
        {
            _bricksDatabase = database;

            _asteroids = new();
            _factory = factory;
        }

        public void AddAsteroid(Vector3Int target)
        {
            if(_bricksDatabase.GetBrickByKey(target) == null)
            {
                throw new NullReferenceException("Target position don't have brick");
            }

            Asteroid instance = _factory.Create(target);
            _asteroids.AddLast(instance);
        }

        public void FlyTick()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.FlyTick();
            }
        }

        public void Crash(Asteroid asteroid)
        {

            _asteroids.Remove(asteroid);
        }
    }
}