using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// ���� �������� ������.
    /// </summary>
    public sealed class BrickMovementTests
    {
        private BrickMovementWrapper _movementWrapper;
        private BricksDatabaseAccess _databaseAccess;

        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            Brick controlledBrick = new(Vector3Int.up * 5, BrickPatterns.LBlock);
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3Int.one);

            _database = new(surface);

            _movementWrapper = new(_database);
            _databaseAccess = new(_database);

            _databaseAccess.ChangeAndAddRecentControllableBrick(controlledBrick);
        }

        /// <summary>
        /// ���� ������� ����� ��� ���������� ������������ � �������������� ������� �� �����������.
        /// </summary>
        [Test]
        public void BrickLowerTestOnMin()
        {
            _movementWrapper.TryMoveBrick(Vector3Int.left);

            _movementWrapper.LowerBrickAndCheckGrounding();

            Assert.AreEqual(new Vector3Int(-1, 4, 0), _database.ControllableBrick.Position);

            _movementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(Vector3Int.left, _database.ControllableBrick.Position);
        }

        /// <summary>
        /// ���� ������� ����� ��� ��������� ������������ � �������������� ������� �� �����������.
        /// </summary>
        [Test]
        public void BrickLowerTestOnMax()
        {
            _movementWrapper.TryMoveBrick(new Vector3Int(3, 0, 2));

            _movementWrapper.LowerBrickAndCheckGrounding();

            Assert.AreEqual(new Vector3Int(3, 4, 2), _database.ControllableBrick.Position);

            _movementWrapper.LowerControllableBrickToGround();

            Assert.AreEqual(new Vector3Int(3, 0, 2), _database.ControllableBrick.Position);
        }

        /// <summary>
        /// ���� ������� ��������� ������������� �� ���������� � ���, ��� ���� ��� ��������� �� �����, ����� ��� �������� ��������.
        /// </summary>
        [Test]
        public void BrickOnGroundExceptionTest()
        {
            _movementWrapper.LowerControllableBrickToGround();

            Assert.Throws(typeof(BrickOnGroundException), () => _movementWrapper.LowerBrickAndCheckGrounding());
            Assert.Throws(typeof(BrickOnGroundException), () => _movementWrapper.LowerControllableBrickToGround());
        }

        /// <summary>
        /// ���� ��� ���������� ������� �������.
        /// </summary>
        [Test]
        public void ComputeWorldPositionTest()
        {
            Assert.AreEqual(new Vector3(1, 6, 1), _database.Surface.GetWorldPosition(new Vector3Int(0, 5, 0)));
        }
        
        /// <summary>
        /// ���� ��� ���������� ������� �� ����� ��� ���������� ������.
        /// </summary>
        [Test]
        public void ComputeFeatureGroundPositionWithoutBlocksTest()
        {
            Assert.AreEqual(0, _database.GetHeightByKey(new Vector2Int(2, 1)));
            Assert.AreEqual(0, _database.GetHeightByPattern(new Brick(Vector3Int.zero, BrickPatterns.OBlock)));
        }

        /// <summary>
        /// ���� �������� ����� �� �����������.
        /// </summary>
        [Test]
        public void BrickMovingInsideSurfaceLimitsTest()
        {
            _movementWrapper.TryMoveBrick(Vector3Int.one);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _database.ControllableBrick.Position);

            _movementWrapper.TryMoveBrick(Vector3Int.one * 2);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _database.ControllableBrick.Position);
        }
    }
}