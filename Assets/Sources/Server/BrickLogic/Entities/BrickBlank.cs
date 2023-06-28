using UnityEngine;

namespace Server.BrickLogic
{
    public readonly struct BrickBlank
    {
        public readonly Vector3Int[] Pattern;
        public readonly Vector3Int[][] PatternRotation;

        public BrickBlank(Vector3Int[] brickPattern, Vector3Int[][] patternRotation)
        {
            Pattern = brickPattern;
            PatternRotation = patternRotation;
        }
    }
}