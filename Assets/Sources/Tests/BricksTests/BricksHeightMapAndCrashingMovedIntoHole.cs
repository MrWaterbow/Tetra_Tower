using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public class BricksHeightMapAndCrashingMovedIntoHole
    {
        private Brick _brick1;
        private Brick _brick2;
        private Brick _brick3;
        private Brick _brick4;

        private BrickMovementWrapper _movementWrapper;
        private BricksCrashWrapper _crashWrapper;
        private BricksDatabaseAccess _databaseAccess;

        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            _brick1 = new(Vector3Int.zero, BrickBlanks.LBrick);
            _brick2 = new(Vector3Int.up, BrickBlanks.OBrick);
            _brick3 = new(Vector3Int.up * 2, BrickBlanks.OBrick);
            _brick4 = new(Vector3Int.up, BrickBlanks.OBrick);
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3Int.one);

            _database = new(surface);

            _movementWrapper = new(_database);
            _crashWrapper = new(_database);
            _databaseAccess = new(_database);

            _databaseAccess.ChangeAndAddRecentControllableBrick(_brick1);
            _movementWrapper.TryMoveBrick(Vector3Int.right * 2);
            _databaseAccess.ChangeAndAddRecentControllableBrick(_brick2);
            _movementWrapper.TryMoveBrick(Vector3Int.right * 2);
            _databaseAccess.ChangeAndAddRecentControllableBrick(_brick3);
            _movementWrapper.TryMoveBrick(Vector3Int.right * 2 + Vector3Int.forward);
            _databaseAccess.ChangeAndAddRecentControllableBrick(_brick4);
            _movementWrapper.TryMoveBrick(Vector3Int.right * 2 + Vector3Int.forward * 2);
        }

        [Test]
        public void MoveBrickIntoHoleTest()
        {
            Assert.AreEqual(new Vector3Int(2, 1, 2), _brick4.Position);
        }

        [Test]
        public void BrickGroundingMovedIntoHole()
        {
            _movementWrapper.LowerBrickAndCheckGrounding();

            Assert.AreEqual(0, _brick4.Position.y);
        }

        [Test]
        public void CorrectGroundingWithBlockMovedIntoHole()
        {
            _movementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(0, _brick4.Position.y);
        }
    }
}