using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BrickMovementTests
    {
        private BrickMovementWrapper _brickMovementWrapper;
        private BricksDatabaseAccess _brickDatabaseAccess;

        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            Brick controlledBrick = new(Vector3Int.up * 5, BrickPatterns.LBlock);
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3Int.one);

            _database = new(surface);

            _brickMovementWrapper = new(_database);
            _brickDatabaseAccess = new(_database);

            _brickDatabaseAccess.ChangeAndAddRecentControllableBrick(controlledBrick);
        }

        [Test]
        public void BrickLowerTest()
        {
            _brickMovementWrapper.LowerBrickAndCheckGrounding();

            Assert.AreEqual(new Vector3Int(0, 4, 0), _database.ControllableBrick.Position);

            _brickMovementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(Vector3Int.zero, _database.ControllableBrick.Position);
        }

        [Test]
        public void ComputeWorldPositionTest()
        {
            Assert.AreEqual(new Vector3(1, 6, 1), _database.Surface.GetWorldPosition(new Vector3Int(0, 5, 0)));
        }

        [Test]
        public void ComputeFeatureGroundPositionTest()
        {
            Assert.AreEqual(new Vector3Int(2, 0, 1), _database.ComputeFeatureGroundPosition(new Vector3Int(2, 4, 1)));
        }

        [Test]
        public void BrickMovingInsideSurfaceLimitsTest()
        {
            _brickMovementWrapper.TryMoveBrick(Vector3Int.one);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _database.ControllableBrick.Position);

            _brickMovementWrapper.TryMoveBrick(Vector3Int.one * 2);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _database.ControllableBrick.Position);
        }
    }
}