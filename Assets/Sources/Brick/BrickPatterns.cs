using UnityEngine;

namespace Sources.BricksLogic
{
    public static class BrickPatterns
    {
        public static readonly BrickPattern LBlock = new Vector3Int[4]
        { 
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left, 
            Vector3Int.left + Vector3Int.forward,
        };
    }
}