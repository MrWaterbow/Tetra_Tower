using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Паттерны блоков.
    /// </summary>
    public static class BrickBlanks
    {
        /// <summary>
        /// Заготовка L блока.
        /// </summary>
        public static readonly BrickBlank LBrick = new(
            new[] {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward, },
            new int[,]
            {
                { 1, 0, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
            });

        /// <summary>
        /// Заготовка O блока.
        /// </summary>
        public static readonly BrickBlank OBrick = new(
            new[] {
            Vector3Int.zero,
            Vector3Int.zero + Vector3Int.forward,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.forward, },
            new int[,]
            {

            });

        /// <summary>
        /// Список всех возможных паттернов блока.
        /// </summary>
        public static readonly BrickBlank[] AllPatterns = new[]
        {
            OBrick,
            LBrick,
        };

        public static class TestBlocks
        {
            public static readonly BrickBlank UpBrick = new(
                new[] {
                Vector3Int.zero,
                Vector3Int.up,
                Vector3Int.right + Vector3Int.up },
                new int[,]
                {

                });
        }
    }
}