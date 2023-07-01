using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public static class BricksPatternRotationBlanks
    {
        public static readonly Vector3Int[] OBlockStaticPattern = new[]
        {
            Vector3Int.zero,
            Vector3Int.forward,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.forward,
        };

        public static readonly Vector3Int[] LBlock0DegressRotated = new[]
        {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward
        };

        public static readonly Vector3Int[] LBlock90DegressRotated = new[]
        {
            Vector3Int.zero,
            Vector3Int.forward,
            Vector3Int.back,
            Vector3Int.forward + Vector3Int.right
        };

        public static readonly Vector3Int[] LBlock180DegressRotated = new[]
        {
            Vector3Int.zero,
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.back
        };

        public static readonly Vector3Int[] LBlock270DegressRotated = new[]
        {
            Vector3Int.zero,
            Vector3Int.back,
            Vector3Int.forward,
            Vector3Int.back + Vector3Int.left
        };
    }
}