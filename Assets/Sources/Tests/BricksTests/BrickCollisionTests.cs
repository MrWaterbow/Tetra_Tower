using NUnit.Framework;
using Server.BrickLogic;
using Server.Factories;
using UnityEngine;

namespace Tests
{
    public sealed class BrickCollisionTests
    {
        private BricksSpace _bricksSpace;

        private Brick _OBrick;
        private Brick _LBrick;
        private Brick _SecondLBrick;

        [SetUp]
        public void Setup()
        {
            _bricksSpace = new(new(Vector2Int.one * 3, Vector3.zero));

            _OBrick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _LBrick = new(Vector3Int.one, BrickPatterns.LBlock);
            _SecondLBrick = new(new Vector3Int(1, 2, 0), BrickPatterns.LBlock);

            _bricksSpace.ChangeAndAddRecentControllableBrick(_OBrick);
            _bricksSpace.ChangeAndAddRecentControllableBrick(_LBrick);
            _bricksSpace.ChangeAndAddRecentControllableBrick(_SecondLBrick);
        }

        [Test]
        public void FirstHeightMapTest()
        {
            Assert.AreEqual(1, _bricksSpace.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(1, _bricksSpace.HeightMap[Vector2Int.right]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[Vector2Int.up]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[Vector2Int.one]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[new Vector2Int(2, 1)]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[Vector2Int.up * 2]);
        }

        [Test]
        public void SecondHeightMapTest()
        {
            // Ставит третий блок
            _bricksSpace.PlaceControllableBrick();

            Assert.AreEqual(3, _bricksSpace.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(3, _bricksSpace.HeightMap[Vector2Int.right]);
            Assert.AreEqual(3, _bricksSpace.HeightMap[Vector2Int.right * 2]);
            Assert.AreEqual(3, _bricksSpace.HeightMap[Vector2Int.up]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[Vector2Int.one]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[new Vector2Int(2, 1)]);
            Assert.AreEqual(2, _bricksSpace.HeightMap[Vector2Int.up * 2]);
        }
    }
}