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
            Brick brick = new(Vector3Int.zero, BrickPatterns.LBlock);

            Assert.AreEqual(2f, _crashWrapper.ComputeDistanceFromNearUnstableTile(Vector3Int.right, brick));
            Assert.AreEqual(1f, _crashWrapper.ComputeDistanceFromNearUnstableTile(Vector3Int.zero, brick));

            Assert.AreEqual(1f, _crashWrapper.ComputeDistanceFromNearStableTile(Vector3Int.left, brick));
            Assert.AreEqual(Mathf.Sqrt(2), _crashWrapper.ComputeDistanceFromNearStableTile(Vector3Int.left + Vector3Int.forward, brick));
        }
    }
}