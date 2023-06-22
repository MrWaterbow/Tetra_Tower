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
        public void ComputeFootFactorTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);

            Assert.AreEqual(1f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.left);

            Assert.AreEqual(0.5f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.back * 2);

            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.up);

            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(brick));
        }

        [Test]
        public void DestroyBlocksWithLowFootFactorTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _database.AddBrick(brick);
            _crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.Bricks.Count);

            brick.ChangePosition(Vector3Int.up);
            _crashWrapper.TryCrashAll();

            Assert.AreEqual(0, _database.Bricks.Count);
        }

        [Test]
        public void HeightMapClearTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _database.AddBrickAndUpdateDatabase(brick);

            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.right));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.up));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.one));

            _crashWrapper.CrashAll();

            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.right));
            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.up));
            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.one));
        }

        [Test]
        public void BrickCrashingTestWithTwoBlocks()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            Brick brick2 = new(Vector3Int.one, BrickPatterns.OBlock);

            _database.AddBrickAndUpdateDatabase(brick);
            _database.AddBrickAndUpdateDatabase(brick2);

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.Bricks.Count);
        }

        [Test]
        public void HeightMapClearingWithTwoBlocksTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            Brick brick2 = new(Vector3Int.one, BrickPatterns.OBlock);

            _database.AddBrickAndUpdateDatabase(brick);
            _database.AddBrickAndUpdateDatabase(brick2);

            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.right));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.up));
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.one));
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.one + Vector2Int.right));
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.one + Vector2Int.up));
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.one * 2));

            _crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.right));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.up));
            Assert.AreEqual(1, _database.GetHeightByKey(Vector2Int.one));
            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.one * 2));
        }
    }
}