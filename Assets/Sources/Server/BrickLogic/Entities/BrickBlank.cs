using UnityEngine;

namespace Server.BrickLogic
{
    public class BrickBlank
    {
        public readonly Vector3Int[] Pattern;
        public readonly int[,] Matrix;

        public BrickBlank(Vector3Int[] brickPattern, int[,] brickMatrix)
        {
            Pattern = brickPattern;
            Matrix = brickMatrix;
        }
    }
}