using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class RotatedBrickCollisionTests
    {
        private BrickMovementWrapper _movementWrapper;
        private BricksDatabaseAccess _databaseAccess;

        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            PlacingSurface placingSurface = new(Vector2Int.one * 3, Vector3.zero);

            _database = new(placingSurface);
            _movementWrapper = new(_database);
            _databaseAccess = new(_database);

            Brick upBlock = new(Vector3Int.zero, BrickPatterns.TestBlocks.UpBlock);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _databaseAccess.PlaceControllableBrick();
        }

        [Test]
        public void OneUpBlockTest()
        {
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(2, _database.GetHeightByKey(Vector2Int.right));
        }

        [Test]
        public void TwoUpBlockTest()
        {
            Brick upBlock = new(Vector3Int.up * 2, BrickPatterns.TestBlocks.UpBlock);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _databaseAccess.PlaceControllableBrick();

            Assert.AreEqual(4, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(4, _database.GetHeightByKey(Vector2Int.right));
        }

        [Test]
        public void CorrectUpBlockFall()
        {
            Brick upBlock = new(Vector3Int.left + Vector3Int.up * 5, BrickPatterns.TestBlocks.UpBlock);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _movementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(Vector3Int.left + Vector3Int.up, upBlock.Position);
        }
    }
}