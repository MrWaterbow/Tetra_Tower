using NUnit.Framework;
using Sources.BricksLogic;
using UnityEngine;

namespace Tests
{
    public sealed class BrickMovementTests
    {
        private BricksSpace _brickSpace;

        [SetUp]
        public void Setup()
        {
            Brick controlledBrick = new(Vector3Int.up * 5, BrickPatterns.LBlock);

            _brickSpace = new(Vector2Int.one * 3, Vector3.one, controlledBrick);
        }

        [Test]
        public void BrickLowerTest()
        {
            _brickSpace.LowerControllableBrick();

            Assert.AreEqual(new Vector3Int(0, 4, 0), _brickSpace.ControllableBlockPosition);
        }

        [Test]
        public void ComputeWorldPositionTest()
        {
            Vector3 worldPosition = _brickSpace.WorldPositionOffset + _brickSpace.ControllableBlockPosition;

            Assert.AreEqual(new Vector3(1, 6, 1), worldPosition);
        }

        [Test]
        public void BrickMovingInsideSurfaceLimitsTest()
        {
            _brickSpace.TryMoveBrick(Vector3Int.one);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _brickSpace.ControllableBlockPosition);

            _brickSpace.TryMoveBrick(Vector3Int.one * 2);

            Assert.AreEqual(new Vector3Int(1, 6, 1), _brickSpace.ControllableBlockPosition);

            // TODO 6.1 ���� ����� ����� �� �������, ������ ���� ��� ��������� � ������� ������ ������
            // ( � ����� ������ ���� ���� �����������, ������� �������� � ������������ ��� ������
        }
    }
}