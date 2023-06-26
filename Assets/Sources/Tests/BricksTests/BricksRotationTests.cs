using NUnit.Framework;
using Server.BrickLogic;
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
        public void RotationTest()
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
        public void NegativeRotationTest()
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
    }
}