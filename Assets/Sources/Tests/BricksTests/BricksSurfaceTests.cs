using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// “ест поверхности на которую став€тьс€ блоки
    /// </summary>
    public sealed class BricksSurfaceTests
    {
        private PlacingSurface _surface;

        private BricksDatabase _database;
        private BrickMovementWrapper _movementWrapper;
        private BricksDatabaseAccess _databaseAccess;
        private BricksCrashWrapper _crashWrapper;

        [SetUp]
        public void Setup()
        {
            _surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new(_surface);
            _movementWrapper = new(_database);
            _databaseAccess = new(_database);
            _crashWrapper = new(_database);
        }

        /// <summary>
        /// “ест метода, который провер€ет находитс€ ли позици€ в лимитах поверхности.
        /// </summary>
        [Test]
        public void PositionsInsideSurfaceLimitsTest()
        {
            Assert.IsTrue(_surface.PositionIntoSurfaceTiles(Vector2Int.one * 2));
            Assert.IsTrue(_surface.PositionIntoSurfaceTiles(Vector2Int.up * 2));
            Assert.IsTrue(_surface.PositionIntoSurfaceTiles(Vector2Int.right * 2));
            Assert.IsTrue(_surface.PositionIntoSurfaceTiles(Vector2Int.zero));

            Assert.IsFalse(_surface.PositionIntoSurfaceTiles(Vector2Int.one * 3));
            Assert.IsFalse(_surface.PositionIntoSurfaceTiles(Vector2Int.right * 3));
            Assert.IsFalse(_surface.PositionIntoSurfaceTiles(Vector2Int.left));
            Assert.IsFalse(_surface.PositionIntoSurfaceTiles(Vector2Int.up * 3));
            Assert.IsFalse(_surface.PositionIntoSurfaceTiles(Vector2Int.down));

            // TODO 6.1 Ѕлок может выйти за границы, только если они расширины с помощью других блоков
            // ( у этого должны быть свои ограничени€, которые задаютс€ в пространстве дл€ блоков
        }

        /// <summary>
        /// “ест метода, который провер€ет находитс€ ли паттерн в лимитах поверхности.
        /// </summary>
        [Test]
        public void PatternsInsideSurfaceLimitsTest()
        {
            Assert.IsTrue(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.one * 2));

            Assert.IsTrue(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.right * 2));
            Assert.IsTrue(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.left));

            Assert.IsTrue(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.up * 2));

            Assert.IsFalse(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.one * 3));

            Assert.IsFalse(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.right * 4));
            Assert.IsFalse(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.left * 2));

            Assert.IsFalse(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.up * 3));
            Assert.IsFalse(_surface.PatternIntoSurfaceTiles(BrickBlanks.LBrick.Pattern, Vector2Int.down));
        }

        [Test]
        public void SurfaceWithoutExtendTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);

            _databaseAccess.SetAndAddRecentControllableBrick(brick);
            _movementWrapper.TryMoveBrick(Vector3Int.left * 2);

            Assert.AreEqual(Vector3Int.zero, brick.Position);
        }

        [Test]
        public void SurfaceExtendTest()
        {
            Brick brick = new(Vector3Int.left, BrickBlanks.OBrick);
            Brick brick2 = new(Vector3Int.up, BrickBlanks.OBrick);

            _databaseAccess.SetAndAddRecentControllableBrick(brick);
            _databaseAccess.SetAndAddRecentControllableBrick(brick2);

            _movementWrapper.TryMoveBrick(Vector3Int.left * 3);

            Assert.AreEqual(Vector3Int.up, brick2.Position);

            _movementWrapper.TryMoveBrick(Vector3Int.left * 2);

            Assert.AreEqual(Vector3Int.left * 2 + Vector3Int.up, brick2.Position);
        }

        [Test]
        public void ClearExtendOnCrashTest()
        {
            Brick brick = new(Vector3Int.left, BrickBlanks.OBrick);

            _databaseAccess.SetAndAddRecentControllableBrick(brick);
            _movementWrapper.LowerControllableBrickToGround();
            _databaseAccess.PlaceControllableBrick();

            Assert.IsTrue(_database.Surface.SurfaceTiles.Contains(Vector2Int.left));
            Assert.IsTrue(_database.Surface.SurfaceTiles.Contains(Vector2Int.left + Vector2Int.up));

            _crashWrapper.CrashAll();

            Assert.IsFalse(_database.Surface.SurfaceTiles.Contains(Vector2Int.left));
            Assert.IsFalse(_database.Surface.SurfaceTiles.Contains(Vector2Int.left + Vector2Int.up));
        }
    }
}