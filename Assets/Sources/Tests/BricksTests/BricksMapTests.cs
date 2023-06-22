using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BricksMapTests
    {
        private BricksDatabase _database;

        private Brick _brick;
        private Brick _brick2;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new(surface);

            _brick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _brick2 = new(Vector3Int.one, BrickPatterns.OBlock);

            _database.AddBrickAndUpdateDatabase(_brick);
            _database.AddBrickAndUpdateDatabase(_brick2);
        }

        [Test]
        public void KeyValueTest()
        {
            Assert.AreEqual(_brick.Position, _database.GetBrickByKey(Vector3Int.zero).Position);
            Assert.AreEqual(_brick.Position, _database.GetBrickByKey(Vector3Int.right).Position);
            Assert.AreEqual(_brick.Position, _database.GetBrickByKey(Vector3Int.forward).Position);
            Assert.AreEqual(_brick.Position, _database.GetBrickByKey(Vector3Int.right + Vector3Int.forward).Position);

            Assert.AreEqual(_brick2.Position, _database.GetBrickByKey(Vector3Int.one).Position);
            Assert.AreEqual(_brick2.Position, _database.GetBrickByKey(Vector3Int.right + Vector3Int.one).Position);
            Assert.AreEqual(_brick2.Position, _database.GetBrickByKey(Vector3Int.forward + Vector3Int.one).Position);
            Assert.AreEqual(_brick2.Position, _database.GetBrickByKey(Vector3Int.right + Vector3Int.forward + Vector3Int.one).Position);
        }
    }
}