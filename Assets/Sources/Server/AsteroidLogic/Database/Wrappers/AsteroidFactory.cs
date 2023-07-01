using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class AsteroidFactory : IAsteroidFactory
    {
        private readonly Vector3Int[] _destroyArea;
        private readonly float _flyTimer;

        public Asteroid Create(Vector3Int target)
        {
            return new(_destroyArea, target, _flyTimer);
        }
    }
}