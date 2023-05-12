using NUnit.Framework;
using Sources.BricksLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public sealed class BricksTests
    {
        private BricksSpace _brickSpace;

        [SetUp]
        public void Setup()
        {
            Brick controlledBrick = new(Vector3Int.up * 5, BrickPatterns.LBlock);

            _brickSpace = new(Vector2Int.one * 3, Vector3.one, new List<Brick>(), controlledBrick);
        }

        [Test]
        public void BrickLowerTest()
        {
            _brickSpace.LowerControlledBrick();

            Assert.AreEqual(new Vector3Int(0, 4, 0), _brickSpace.ControlledBlockPosition);
        }

        [Test]
        public void ComputeWorldPositionTest()
        {
            Vector3 worldPosition = _brickSpace.WorldPositionOffset + _brickSpace.ControlledBlockPosition;

            Assert.AreEqual(new Vector3(1, 6, 1), worldPosition);
        }

        [Test]
        public void BrickMovementTest()
        {
            _brickSpace.TryMoveControlledBrick(Vector3Int.one);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _brickSpace.ControlledBlockPosition);

            _brickSpace.TryMoveControlledBrick(Vector3Int.one * 2);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _brickSpace.ControlledBlockPosition);
        }
    }
}