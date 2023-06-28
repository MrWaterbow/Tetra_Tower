using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public class BricksRotatingTests
    {
        private BricksDatabase _database;
        private BricksDatabaseAccess _databaseAccess;
        private BrickMovementWrapper _movementWrapper;
        private BricksRotatingWrapper _rotateWrapper;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new BricksDatabase(surface);
            _databaseAccess = new(_database);
            _movementWrapper = new(_database);
            _rotateWrapper = new(_database);
        }

        [Test]
        public void RotationLimitIfNotInSurfaceTest()
        {
            Brick LBrick = new(Vector3Int.zero, BrickBlanks.LBrick);

            _databaseAccess.SetAndAddRecentControllableBrick(LBrick);
            Assert.IsTrue(_rotateWrapper.PossibleRotateBrick());

            _movementWrapper.TryMoveBrick(Vector3Int.right * 3);
            Assert.IsFalse(_rotateWrapper.PossibleRotateBrick());
        }
    }
}