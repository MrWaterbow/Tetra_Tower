using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public readonly struct BrickBlank
    {
        public readonly LinkedList<Vector3Int> Pattern;
        public readonly Vector3Int[][] PatternRotation;

        public BrickBlank(Vector3Int[] brickPattern, Vector3Int[][] patternRotation)
        {
            Pattern = new();

            foreach (Vector3Int tile in brickPattern)
            {
                Pattern.AddLast(tile);
            }

            PatternRotation = patternRotation;
        }
    }
}