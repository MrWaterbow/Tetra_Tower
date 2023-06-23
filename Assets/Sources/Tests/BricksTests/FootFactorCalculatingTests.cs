using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class FootFactorCalculatingTests
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
        public void CalculateFootFactorByGroundWithoutBricksTest()
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
        public void CalculateFootFactorWithBricksTest()
        {
            Brick groundBrick = new(Vector3Int.zero, BrickPatterns.OBlock);
            Brick groundBrick2 = new(Vector3Int.right * 2, BrickPatterns.OBlock);
            Brick mainBrick = new(Vector3Int.forward + Vector3Int.up, BrickPatterns.OBlock);

            _database.AddBrickAndUpdateDatabase(groundBrick);
            _database.AddBrickAndUpdateDatabase(groundBrick2);

            Assert.AreEqual(0.5f, _crashWrapper.ComputeFootFactor(mainBrick));

            mainBrick.ChangePosition(Vector3Int.forward + Vector3Int.right * 3 + Vector3Int.up);

            Assert.AreEqual(0.25f, _crashWrapper.ComputeFootFactor(mainBrick));
        }

        [Test]
        public void CalculateFootFactorWithDistanceCheckingWithoutBricks()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.LBlock);
        }
    }
}