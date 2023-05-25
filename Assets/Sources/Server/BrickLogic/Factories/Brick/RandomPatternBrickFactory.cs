using Server.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public sealed class RandomPatternBrickFactory : IBrickFactory
    {
        private readonly Vector3Int[][] _patterns;

        public RandomPatternBrickFactory(Vector3Int[][] patterns)
        {
            _patterns = patterns;
        }

        public Brick Create(Vector3Int position)
        {
            return new Brick(position, _patterns[Random.Range(0, _patterns.Length)]);
        }
    }
}