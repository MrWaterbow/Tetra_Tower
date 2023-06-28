using UnityEngine;

namespace Server.BrickLogic
{
    public readonly struct BrickBlank
    {
        public readonly Vector3Int[] Pattern;
        public readonly int[,] Matrix;
        public readonly Vector2Int Center;

        public BrickBlank(Vector3Int[] brickPattern, int[,] brickMatrix, Vector2Int center)
        {
            Pattern = brickPattern;
            Matrix = brickMatrix;
            Center = center;
        }
    }
}