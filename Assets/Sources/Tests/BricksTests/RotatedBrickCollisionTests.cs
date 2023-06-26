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

            Brick upBlock = new(Vector3Int.zero, BrickBlanks.TestBlocks.UpBrick);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _databaseAccess.PlaceControllableBrick();
        }

        [Test]
        public void CorrectUpBlockFastLowering()
        {
            Brick upBlock = new(Vector3Int.left + Vector3Int.up * 2, BrickBlanks.TestBlocks.UpBrick);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _movementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(Vector3Int.left + Vector3Int.up, upBlock.Position);
        }

        [Test]
        public void CorrectUpBlockLowering()
        {
            Brick upBlock = new(Vector3Int.left + Vector3Int.up * 2, BrickBlanks.TestBlocks.UpBrick);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBlock);
            _movementWrapper.LowerBrickAndCheckGrounding();
            _movementWrapper.LowerBrickAndCheckGrounding();
            _movementWrapper.LowerBrickAndCheckGrounding();

            Assert.AreEqual(Vector3Int.left + Vector3Int.up, upBlock.Position);
        }
    }
}