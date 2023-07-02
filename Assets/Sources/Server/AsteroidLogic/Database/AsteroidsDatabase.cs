using Server.BrickLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class AsteroidsDatabase
    {
        private readonly BricksDatabase _bricksDatabase;

        private readonly HashSet<Asteroid> _asteroids;
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
            _asteroids.Add(instance);
        }

        public void FlyTick()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.FlyTick();
            }

            CheckCrashing();
        }

        public void AllToTargets()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.ToTarget();
            }

            CheckCrashing();
        }

        public void CheckCrashing()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                if(asteroid.IsReachedTarget)
                {
                    CrashTiles(asteroid);
                }
            }

            _bricksDatabase.UpdateBricks();

            _asteroids.RemoveWhere(asteroid => asteroid.IsReachedTarget);
        }

        private void CrashTiles(Asteroid asteroid)
        {
            foreach (Vector3Int destroyTile in asteroid.DestroyArea)
            {
                _bricksDatabase.RemoveTile(destroyTile + asteroid.Target);
            }
        }

        public Vector3Int TryGetRandomBricksMapKey()
        {
            return _bricksDatabase.TryGetRandomBricksMapKey();
        }
    }
}