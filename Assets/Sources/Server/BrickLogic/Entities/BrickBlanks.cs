using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickBlank
    {
        public readonly Vector3Int[] BrickPattern;
        public readonly int[,] BrickMatrix;

        public BrickBlank(Vector3Int[] brickPattern, int[,] brickMatrix)
        {
            BrickPattern = brickPattern;
            BrickMatrix = brickMatrix;
        }
    }

    /// <summary>
    /// Паттерны блоков.
    /// </summary>
    public static class BrickBlanks
    {
        /// <summary>
        /// Заготовка L блока.
        /// </summary>
        public static readonly BrickBlank LBlock = new(
            new[] {
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.forward, },
            new int[,]
            {

            });

        /// <summary>
        /// Заготовка O блока.
        /// </summary>
        public static readonly BrickBlank OBlock = new(
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
            OBlock,
            LBlock,
        };

        public static class TestBlocks
        {
            public static readonly BrickBlank UpBlock = new(
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