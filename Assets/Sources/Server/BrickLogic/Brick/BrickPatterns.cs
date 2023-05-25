using UnityEngine;

namespace Server.BricksLogic
{
    public static class BrickPatterns
    {
        public static readonly Vector3Int[][] AllPatterns = new Vector3Int[1][]
        {
            LBlock,
        };

        public static readonly Vector3Int[] LBlock = new Vector3Int[4]
        {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward,
        };
    }
}