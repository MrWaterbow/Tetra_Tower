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

        [SetUp]
        public void Setup()
        {
            _surface = new(Vector2Int.one * 3, Vector3.zero);
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
    }
}