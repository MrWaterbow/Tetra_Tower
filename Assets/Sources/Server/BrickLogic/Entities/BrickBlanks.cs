using UnityEngine;
using RotationBlank = Server.BrickLogic.BricksPatternRotationBlanks;

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
            new Vector3Int[][]
            {
                RotationBlank.LBlock0DegressRotated,
                RotationBlank.LBlock90DegressRotated,
                RotationBlank.LBlock180DegressRotated,
                RotationBlank.LBlock270DegressRotated
            });
            

        /// <summary>
        /// Заготовка O блока.
        /// </summary>
        public static readonly BrickBlank OBrick = new(
            RotationBlank.OBlockStaticPattern,
            new Vector3Int[][]
            {
                RotationBlank.OBlockStaticPattern
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
                new Vector3Int[1][]);
        }
    }
}