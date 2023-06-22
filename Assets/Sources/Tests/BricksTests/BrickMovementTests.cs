using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// “ест движени€ блоков.
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
        /// “ест падени€ блока при наименьшей вертикальной и горизонтальной позиции на поверхности.
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
        /// “ест падени€ блока при наивысшей вертикальной и горизонтальной позиции на поверхности.
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
        /// “ест который провер€ют выбрасываютс€ ли исключени€ о том, что блок уже находитс€ на земле, когда его пытаютс€ опустить.
        /// </summary>
        [Test]
        public void BrickOnGroundExceptionTest()
        {
            _movementWrapper.LowerControllableBrickToGround();

            Assert.Throws(typeof(BrickOnGroundException), () => _movementWrapper.LowerBrickAndCheckGrounding());
            Assert.Throws(typeof(BrickOnGroundException), () => _movementWrapper.LowerControllableBrickToGround());
        }

        /// <summary>
        /// “ест дл€ вычислени€ мировой позиции.
        /// </summary>
        [Test]
        public void ComputeWorldPositionTest()
        {
            Assert.AreEqual(new Vector3(1, 6, 1), _database.Surface.GetWorldPosition(new Vector3Int(0, 5, 0)));
        }
        
        /// <summary>
        /// “ест дл€ вычислени€ позиции на земле при отсутствии блоков.
        /// </summary>
        [Test]
        public void ComputeFeatureGroundPositionWithoutBlocksTest()
        {
            Assert.AreEqual(0, _database.GetHeightByKey(new Vector2Int(2, 1)));
            Assert.AreEqual(0, _database.GetHeightByPattern(new Brick(Vector3Int.zero, BrickPatterns.OBlock)));
        }

        [Test]
        public void MoveIntoBrickTest()
        {
            Brick brick2 = new(new Vector3Int(2, 1), BrickPatterns.OBlock);
            Brick brick3 = new(new Vector3Int(1, 1), BrickPatterns.OBlock);

            _movementWrapper.TryMoveBrick(Vector3Int.right * 2);
            _databaseAccess.ChangeAndAddRecentControllableBrick(brick2);
            _databaseAccess.ChangeAndAddRecentControllableBrick(brick3);
            _movementWrapper.TryMoveBrick(Vector3Int.right);

            Assert.AreEqual(new Vector3Int(1, 1), brick3.Position);
        }

        /// <summary>
        /// “ест движени€ блока по поверхности.
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