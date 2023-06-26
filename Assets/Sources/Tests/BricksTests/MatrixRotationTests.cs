using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;
using Blanks = Server.BrickLogic.BricksMatrixRotationBlanks;

namespace Tests
{
    public sealed class MatrixRotationTests
    {
        [Test]
        public void MatrixLeightTest()
        {
            Assert.AreEqual(3, MatrixColumnLength(BrickBlanks.LBrick.Matrix.Length));
        }

        [Test]
        public void MatrixYRotationTest()
        {
            int[,] matrix = BrickBlanks.LBrick.Matrix;
            int length = MatrixColumnLength(matrix.Length);

            int[,] tempMatrix = Rotate90DegressMatrix(matrix, length);

            Assert.AreEqual(Blanks.LBlock90DegressRotatedMatrix, tempMatrix);

            tempMatrix = Rotate90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock180DegressRotatedMatrix, tempMatrix);

            tempMatrix = Rotate90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock270DegressRotatedMatrix, tempMatrix);

            tempMatrix = Rotate90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock0DegressRotatedMatrix, tempMatrix);
        }

        [Test]
        public void MatrixYNegativeRotationTest()
        {
            int[,] matrix = BrickBlanks.LBrick.Matrix;
            int length = MatrixColumnLength(matrix.Length);

            int[,] tempMatrix = RotateMinus90DegressMatrix(matrix, length);

            Assert.AreEqual(Blanks.LBlock270DegressRotatedMatrix, tempMatrix);

            tempMatrix = RotateMinus90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock180DegressRotatedMatrix, tempMatrix);

            tempMatrix = RotateMinus90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock90DegressRotatedMatrix, tempMatrix);

            tempMatrix = RotateMinus90DegressMatrix(tempMatrix, length);

            Assert.AreEqual(Blanks.LBlock0DegressRotatedMatrix, tempMatrix);
        }

        private int[,] Rotate90DegressMatrix(int[,] matrix, int length)
        {
            int[,] rotatedMatrix = new int[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    rotatedMatrix[i, j] = matrix[length - j - 1, i];
                }
            }

            return rotatedMatrix;
        }

        private int[,] RotateMinus90DegressMatrix(int[,] matrix, int length)
        {
            int[,] rotatedMatrix = new int[length, length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    rotatedMatrix[length - j - 1, i] = matrix[i, j];
                }
            }

            return rotatedMatrix;
        }

        private int MatrixColumnLength(int length)
        {
            return (int)Mathf.Sqrt(length);
        }
    }
}