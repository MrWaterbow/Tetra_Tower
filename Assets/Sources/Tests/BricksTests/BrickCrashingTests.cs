using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BrickCrashingTests
    {
        private BricksCrashWrapper _crashWrapper;
        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new BricksDatabase(surface);

            _crashWrapper = new(_database);
        }

        [Test]
        public void DestroyBlocksWithLowFootFactorTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);
            _database.AddBrick(brick);
            _crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.Bricks.Count);

            brick.ChangePosition(Vector3Int.up);
            _crashWrapper.TryCrashAll();

            Assert.AreEqual(0, _database.Bricks.Count);
        }

        [Test]
        public void BrickCrashingTestWithTwoBlocks()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);
            Brick brick2 = new(Vector3Int.one, BrickBlanks.OBrick);

            _database.AddBrickAndUpdateDatabase(brick);
            _database.AddBrickAndUpdateDatabase(brick2);

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.Bricks.Count);
        }

        [Test]
        public void CrashingByGroundTest()
        {
            Brick brick = new(Vector3Int.left * 2, BrickBlanks.OBrick);
            _database.AddBrickAndUpdateDatabase(brick);

            Assert.AreEqual(1, _database.Bricks.Count);

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(0, _database.Bricks.Count);
        }

        [Test]
        public void GroupCrashingTest()
        {
            Brick OBrick = new(Vector3Int.left, BrickBlanks.OBrick);
            Brick LBrick = new(Vector3Int.left + Vector3Int.up, BrickBlanks.LBrick);
            _database.AddBrickAndUpdateDatabase(OBrick);
            _database.AddBrickAndUpdateDatabase(LBrick);

            Assert.AreEqual(2, _database.Bricks.Count);

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(0, _database.Bricks.Count);
            Assert.AreEqual(0, _database.GetHeighestPoint());
        }

        [Test]
        public void NegativeSupportCrashTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);
            Brick OBrick = new(Vector3Int.left * 2 + Vector3Int.up, BrickBlanks.OBrick);

            _database.AddBrickAndUpdateDatabase(LBrick);
            _database.AddBrickAndUpdateDatabase(OBrick);

            Assert.AreEqual(2, _database.Bricks.Count);

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(0, _database.Bricks.Count);
        }
    }
}