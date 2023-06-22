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

        [SetUp]
        public void Setup()
        {
            _surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new(_surface);
            _movementWrapper = new(_database);
            _databaseAccess = new(_database);
        }

        /// <summary>
        /// “ест метода, который провер€ет находитс€ ли позици€ в лимитах поверхности.
        /// </summary>
        [Test]
        public void PositionsInsideSurfaceLimitsTest()
        {
            Assert.IsTrue(_surface.PositionInSurfaceLimits(Vector2Int.one * 2));
            Assert.IsTrue(_surface.PositionInSurfaceLimits(Vector2Int.up * 2));
            Assert.IsTrue(_surface.PositionInSurfaceLimits(Vector2Int.right * 2));
            Assert.IsTrue(_surface.PositionInSurfaceLimits(Vector2Int.zero));

            Assert.IsFalse(_surface.PositionInSurfaceLimits(Vector2Int.one * 3));
            Assert.IsFalse(_surface.PositionInSurfaceLimits(Vector2Int.right * 3));
            Assert.IsFalse(_surface.PositionInSurfaceLimits(Vector2Int.left));
            Assert.IsFalse(_surface.PositionInSurfaceLimits(Vector2Int.up * 3));
            Assert.IsFalse(_surface.PositionInSurfaceLimits(Vector2Int.down));

            // TODO 6.1 Ѕлок может выйти за границы, только если они расширины с помощью других блоков
            // ( у этого должны быть свои ограничени€, которые задаютс€ в пространстве дл€ блоков
        }

        /// <summary>
        /// “ест метода, который провер€ет находитс€ ли паттерн в лимитах поверхности.
        /// </summary>
        [Test]
        public void PatternsInsideSurfaceLimitsTest()
        {
            Assert.IsTrue(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.one * 2));

            Assert.IsTrue(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.right * 2));
            Assert.IsTrue(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.left));

            Assert.IsTrue(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.up * 2));

            Assert.IsFalse(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.one * 3));

            Assert.IsFalse(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.right * 4));
            Assert.IsFalse(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.left * 2));

            Assert.IsFalse(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.up * 3));
            Assert.IsFalse(_surface.PatternInSurfaceLimits(BrickPatterns.LBlock, Vector2Int.down));
        }

        [Test]
        public void SurfaceWithoutExtendTest()
        {
            Brick brick = new(Vector3Int.zero, BrickPatterns.OBlock);

            _databaseAccess.ChangeAndAddRecentControllableBrick(brick);
            _movementWrapper.TryMoveBrick(Vector3Int.left * 2);

            Assert.AreEqual(Vector3Int.zero, brick.Position);
        }

        [Test]
        public void SurfaceExtendTest()
        {
            Brick brick = new(Vector3Int.left, BrickPatterns.OBlock);
            Brick brick2 = new(Vector3Int.zero, BrickPatterns.OBlock);

            _databaseAccess.ChangeAndAddRecentControllableBrick(brick);
            _databaseAccess.ChangeAndAddRecentControllableBrick(brick2);

            _movementWrapper.TryMoveBrick(Vector3Int.left * 3);

            Assert.AreEqual(Vector3Int.zero, brick2.Position);

            _movementWrapper.TryMoveBrick(Vector3Int.left * 2);

            Assert.AreEqual(Vector3Int.left * 2, brick2.Position);
        }
    }
}