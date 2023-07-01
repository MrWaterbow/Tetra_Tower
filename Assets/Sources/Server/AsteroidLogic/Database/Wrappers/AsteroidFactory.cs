using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class AsteroidFactory : IAsteroidFactory
    {
        private readonly Vector3Int[] _destroyArea;
        private readonly float _flyTimer;

        public AsteroidFactory(Vector3Int[] destroyArea, float flyTimer)
        {
            _destroyArea = destroyArea;
            _flyTimer = flyTimer;
        }

        public Asteroid Create(Vector3Int target)
        {
            return new(_destroyArea, target, _flyTimer);
        }
    }
}