using UnityEngine;

namespace Server.BricksLogic
{
    public static class BrickPatterns
    {
        /// <summary>
        /// Заготовка L блока
        /// </summary>
        public static readonly Vector3Int[] LBlock = new Vector3Int[4]
        {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward,
        };

        /// <summary>
        /// Список всех возможных паттернов блока
        /// </summary>
        public static readonly Vector3Int[][] AllPatterns = new Vector3Int[1][]
        {
            LBlock,
        };
    }
}