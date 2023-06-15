﻿using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Тесты коллизий блока. Под коллизиями подразумевается карта высот.
    /// </summary>
    public sealed class BrickCollisionTests
    {
        private BrickMovementWrapper _brickMovementWrapper;
        private BricksDatabaseAccess _bricksDatabaseAccess;

        private BricksDatabase _database;

        private Brick _OBrick;
        private Brick _LBrick;
        private Brick _SecondLBrick;

        [SetUp]
        public void Setup()
        {
            PlacingSurface placingSurface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new BricksDatabase(placingSurface);

            _brickMovementWrapper = new(_database);
            _bricksDatabaseAccess = new(_database);

            _OBrick = new(Vector3Int.zero, BrickPatterns.OBlock);
            _LBrick = new(Vector3Int.one, BrickPatterns.LBlock);
            _SecondLBrick = new(new Vector3Int(1, 2, 0), BrickPatterns.LBlock);

            _bricksDatabaseAccess.ChangeAndAddRecentControllableBrick(_OBrick);
            _bricksDatabaseAccess.ChangeAndAddRecentControllableBrick(_LBrick);
            _bricksDatabaseAccess.ChangeAndAddRecentControllableBrick(_SecondLBrick);
        }

        /// <summary>
        /// Тест будущей позицию блока, если бы он находился на земле.
        /// </summary>
        [Test]
        public void CalculateFutureGroundPositionWithBlocksTest()
        {
            _bricksDatabaseAccess.PlaceControllableBrick();

            Assert.AreEqual(3, _database.GetHeightByKey(Vector2Int.zero));
            Assert.AreEqual(3, _database.GetHeightByKey(Vector2Int.right * 2));
            Assert.AreEqual(0, _database.GetHeightByKey(Vector2Int.one * 2));

            Brick testInstance = new(Vector3Int.zero, BrickPatterns.OBlock);
            Assert.AreEqual(3, _database.GetHeightByPattern(testInstance));

            testInstance.ChangePosition(Vector3Int.right * 2);
            Assert.AreEqual(3, _database.GetHeightByPattern(testInstance));
        }

        /// <summary>
        /// Тест карты высот при двух поставленных блоков.
        /// </summary>
        [Test]
        public void HeightMapTestOnTwoBlocks()
        {
            Assert.AreEqual(1, _database.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(1, _database.HeightMap[Vector2Int.right]);
            Assert.AreEqual(2, _database.HeightMap[Vector2Int.up]);
            Assert.AreEqual(2, _database.HeightMap[Vector2Int.one]);
            Assert.AreEqual(2, _database.HeightMap[new Vector2Int(2, 1)]);
            Assert.AreEqual(2, _database.HeightMap[Vector2Int.up * 2]);
        }

        /// <summary>
        /// Тест карты высот при трех поставленных блоках.
        /// </summary>
        [Test]
        public void HeightMapTestOnThreeBlocks()
        {
            // Ставит третий блок
            _bricksDatabaseAccess.PlaceControllableBrick();

            Assert.AreEqual(3, _database.HeightMap[Vector2Int.zero]);
            Assert.AreEqual(3, _database.HeightMap[Vector2Int.right]);
            Assert.AreEqual(3, _database.HeightMap[Vector2Int.right * 2]);
            Assert.AreEqual(3, _database.HeightMap[Vector2Int.up]);
            Assert.AreEqual(2, _database.HeightMap[Vector2Int.one]);
            Assert.AreEqual(2, _database.HeightMap[new Vector2Int(2, 1)]);
            Assert.AreEqual(2, _database.HeightMap[Vector2Int.up * 2]);
        }

        /// <summary>
        /// Тест метода, который расчитывает наивысшую точку на карте высот.
        /// </summary>
        [Test]
        public void HeighestPointTest()
        {
            Assert.AreEqual(2, _database.GetHeighestPoint());

            _bricksDatabaseAccess.PlaceControllableBrick();

            Assert.AreEqual(3, _database.GetHeighestPoint());
        }
    }
}