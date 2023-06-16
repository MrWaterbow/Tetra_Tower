using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BrickCrashingTests
    {
        private BrickCrashWrapper _crashWrapper;
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
    }
}