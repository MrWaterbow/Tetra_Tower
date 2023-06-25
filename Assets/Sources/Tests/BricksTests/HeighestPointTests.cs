using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Тесты коллизий блока. Под коллизиями подразумевается карта высот.
    /// </summary>
    public sealed class HeighestPointTests
    {
        private BricksDatabaseAccess _databaseAccess;

        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            PlacingSurface placingSurface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new(placingSurface);

            _databaseAccess = new(_database);
        }

        /// <summary>
        /// Тест метода, который расчитывает наивысшую точку на карте высот.
        /// </summary>
        [Test]
        public void HeighestPointTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBlock.BrickPattern);
            Brick upBrick = new(Vector3Int.up, BrickBlanks.TestBlocks.UpBlock.BrickPattern);

            Assert.AreEqual(0, _database.HeighestPoint);

            _databaseAccess.ChangeAndAddRecentControllableBrick(brick);
            _databaseAccess.ChangeAndAddRecentControllableBrick(upBrick);

            Assert.AreEqual(1, _database.HeighestPoint);

            _databaseAccess.PlaceControllableBrick();

            Assert.AreEqual(3, _database.HeighestPoint);
        }
    }
}