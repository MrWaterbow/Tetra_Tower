using NUnit.Framework;
using Server.BrickLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Blanks = Server.BrickLogic.BricksMatrixRotationBlanks;

namespace Tests
{
    public sealed class BricksRotationTests
    {
        private BricksDatabase _database;

        private BricksRotatingWrapper _rotatingWrapper;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new(surface);

            _rotatingWrapper = new(_database);
        }

        [Test]
        public void ColumnLeightComputingTest()
        {
            _database.ControllableBrick = new(Vector3Int.zero, BrickBlanks.LBrick);

            Assert.AreEqual(3, _database.ControllableBrick.MatrixColumnLength);
        }

        [Test]
        public void CenterRecoringTest()
        {
            BrickBlank LBrick = BrickBlanks.LBrick;
            Vector2Int Center = LBrick.Center;

            Assert.AreEqual(1, LBrick.Matrix[Center.x, Center.y]);
            Assert.AreEqual(1, LBrick.Matrix[Center.y, 0]);
            Assert.AreEqual(0, LBrick.Matrix[0, Center.x]);
        }

        [Test]
        public void MatrixRotationTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);
            _database.ControllableBrick = LBrick;

            _rotatingWrapper.TryRotate90();

            Assert.AreEqual(Blanks.LBlock90DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotate90();

            Assert.AreEqual(Blanks.LBlock180DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotate90();

            Assert.AreEqual(Blanks.LBlock270DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotate90();

            Assert.AreEqual(Blanks.LBlock0DegressRotatedMatrix, _database.ControllableBrick.Matrix);
        }

        [Test]
        public void NegativeMatrixRotationTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);
            _database.ControllableBrick = LBrick;

            _rotatingWrapper.TryRotateMinus90();

            Assert.AreEqual(Blanks.LBlock270DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotateMinus90();

            Assert.AreEqual(Blanks.LBlock180DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotateMinus90();

            Assert.AreEqual(Blanks.LBlock90DegressRotatedMatrix, _database.ControllableBrick.Matrix);

            _rotatingWrapper.TryRotateMinus90();

            Assert.AreEqual(Blanks.LBlock0DegressRotatedMatrix, _database.ControllableBrick.Matrix);
        }

        [Test]
        public void PatternRotationTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);
            _database.ControllableBrick = LBrick;

            Assert.AreEqual(Vector3Int.zero, LBrick.Pattern[0]);
            Assert.AreEqual(Vector3Int.right, LBrick.Pattern[1]);
            Assert.AreEqual(Vector3Int.left, LBrick.Pattern[2]);
            Assert.AreEqual(Vector3Int.left + Vector3Int.forward, LBrick.Pattern[3]);

            Assert.AreEqual(4, LBrick.Pattern.Length);

            _rotatingWrapper.TryRotate90();

            //{ 0, 1, 1 },
            //{ 0, 1, 0 },
            //{ 0, 1, 0 }

            Assert.AreEqual(Vector3Int.forward, LBrick.Pattern[0]);
            Assert.AreEqual(Vector3Int.zero, LBrick.Pattern[1]);
            Assert.AreEqual(Vector3Int.back, LBrick.Pattern[2]);
            Assert.AreEqual(Vector3Int.forward + Vector3Int.right, LBrick.Pattern[3]);

            Assert.AreEqual(4, LBrick.Pattern.Length);
        }

        [Test]
        public void IndicesSearchingTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);

            Vector2Int[] indices = LBrick.GetIndicesOfMatrix().ToArray();

            //{ 1, 0, 0 },
            //{ 1, 1, 1 },
            //{ 0, 0, 0 }

            Assert.AreEqual(4, indices.Length);

            Assert.AreEqual(Vector2Int.zero, indices[0]);
            Assert.AreEqual(Vector2Int.right, indices[1]);
            Assert.AreEqual(Vector2Int.up + Vector2Int.right, indices[2]);
            Assert.AreEqual(Vector2Int.up * 2 + Vector2Int.right, indices[3]);
        }
    }
}