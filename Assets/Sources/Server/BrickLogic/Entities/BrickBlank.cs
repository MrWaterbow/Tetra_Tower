using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public readonly struct BrickBlank
    {
        public readonly LinkedList<Vector3Int> Pattern;
        public readonly Vector3Int[][] PatternRotation;

        public BrickBlank(LinkedList<Vector3Int> brickPattern, Vector3Int[][] patternRotation)
        {
            Pattern = brickPattern;
            PatternRotation = patternRotation;
        }
    }
}