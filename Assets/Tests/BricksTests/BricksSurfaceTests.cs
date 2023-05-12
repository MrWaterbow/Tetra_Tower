using NUnit.Framework;
using Sources.BricksLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BricksSurfaceTests
    {
        private BricksSurface _surface;

        [SetUp]
        public void Setup()
        {
            _surface = new(Vector2Int.one * 3, Vector3.zero);
        }

        [Test]
        public void PositionOnSurfaceTest()
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
        }

        [Test]
        public void PatternOnSurfaceTest()
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
    }
}