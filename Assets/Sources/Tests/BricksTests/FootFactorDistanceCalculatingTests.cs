using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class FootFactorDistanceCalculatingTests
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
        public void DistanceCalculatingWithoutBlocksTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.LBlock.BrickPattern);

            Assert.AreEqual(2f, _crashWrapper.ComputeDistanceFromNearUnstableTile(Vector3Int.right, brick));
            Assert.AreEqual(1f, _crashWrapper.ComputeDistanceFromNearUnstableTile(Vector3Int.zero, brick));

            Assert.AreEqual(1f, _crashWrapper.ComputeDistanceFromNearStableTile(Vector3Int.left, brick));
            Assert.AreEqual(Mathf.Sqrt(2), _crashWrapper.ComputeDistanceFromNearStableTile(Vector3Int.left + Vector3Int.forward, brick));

            brick.ChangePosition(Vector3Int.left);

            Vector3Int tilePosition = Vector3Int.left * 2 + Vector3Int.forward;
            Assert.AreEqual(Vector3Int.Distance(Vector3Int.zero, tilePosition), _crashWrapper.ComputeDistanceFromNearStableTile(tilePosition, brick));
        }
    }
}