using UnityEngine;

namespace Server.BricksLogic
{
    public static class BrickPatterns
    {
        /// <summary>
        /// Заготовка L блока
        /// </summary>
        public static readonly Vector3Int[] LBlock = new Vector3Int[]
        {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward,
        };

        public static readonly Vector3Int[] OBlock = new Vector3Int[]
        {
            Vector3Int.zero,
            Vector3Int.zero + Vector3Int.forward,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.forward,
        };

        /// <summary>
        /// Список всех возможных паттернов блока
        /// </summary>
        public static readonly Vector3Int[][] AllPatterns = new Vector3Int[][]
        {
            //LBlock,
            OBlock
        };
    }
}