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
            _database.AddBrickAndUpdateHeightMap(brick);

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
            _database.AddBrickAndUpdateHeightMap(brick);
            _crashWrapper.TestForCrash();

            Assert.AreEqual(1, _database.Bricks.Count);

            brick.ChangePosition(Vector3Int.up);
            _crashWrapper.TestForCrash();

            Assert.AreEqual(0, _database.Bricks.Count);
        }

        [Test]
        public void HeightMapClearTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _database.AddBrickAndUpdateHeightMap(brick);

            Assert.AreEqual(1, _database.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(1, _database.HeightMap[Vector2Int.right]);
            Assert.AreEqual(1, _database.HeightMap[Vector2Int.up]);
            Assert.AreEqual(1, _database.HeightMap[Vector2Int.one]);

            brick.ChangePosition(Vector3Int.up);
            _crashWrapper.TestForCrash();

            Assert.AreEqual(0, _database.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(0, _database.HeightMap[Vector2Int.right]);
            Assert.AreEqual(0, _database.HeightMap[Vector2Int.up]);
            Assert.AreEqual(0, _database.HeightMap[Vector2Int.one]);
        }

        [Test]
        public void HeightMapWithBlocksCrashingTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            Brick secondBrick = new(Vector3Int.one, BrickPatterns.OBlock);

            _database.AddBrickAndUpdateHeightMap(brick);
            _database.AddBrickAndUpdateHeightMap(secondBrick);

            _crashWrapper.TestForCrash();

            Assert.AreEqual(1, _database.Bricks.Count);
        }
    }
}