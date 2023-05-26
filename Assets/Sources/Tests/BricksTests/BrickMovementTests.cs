using NUnit.Framework;
using Server.BricksLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BrickMovementTests
    {
        private BricksSpace _bricksSpace;

        [SetUp]
        public void Setup()
        {
            Brick controlledBrick = new(Vector3Int.up * 5, BrickPatterns.LBlock);
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3Int.one);

            _bricksSpace = new(surface);

            _bricksSpace.ChangeAndAddRecentControllableBrick(controlledBrick);
        }

        [Test]
        public void BrickLowerTest()
        {
            _bricksSpace.LowerBrickAndCheckGrounding();

            Assert.AreEqual(new Vector3Int(0, 4, 0), _bricksSpace.ControllableBrick.Position);
        }

        [Test]
        public void ComputeWorldPositionTest()
        {
            Assert.AreEqual(new Vector3(1, 6, 1), _bricksSpace.Surface.GetWorldPosition(_bricksSpace.ControllableBrick.Position));
        }

        [Test]
        public void BrickMovingInsideSurfaceLimitsTest()
        {
            _bricksSpace.TryMoveBrick(Vector3Int.one);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _bricksSpace.ControllableBrick.Position);

            _bricksSpace.TryMoveBrick(Vector3Int.one * 2);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _bricksSpace.ControllableBrick.Position);

            // TODO 6.1 Блок может выйти за границы, только если они расширины с помощью других блоков
            // ( у этого должны быть свои ограничения, которые задаются в пространстве для блоков
        }
    }
}