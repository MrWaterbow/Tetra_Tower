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
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);
            Brick upBrick = new(Vector3Int.up, BrickBlanks.TestBlocks.UpBrick);

            Assert.AreEqual(0, _database.HeighestPoint);

            _databaseAccess.SetAndAddRecentControllableBrick(brick);
            _databaseAccess.SetAndAddRecentControllableBrick(upBrick);

            Assert.AreEqual(1, _database.HeighestPoint);

            _databaseAccess.PlaceControllableBrick();

            Assert.AreEqual(3, _database.HeighestPoint);
        }

        [Test]
        public void HeighestPointReduceBug()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);
            Brick brick2 = new(Vector3Int.right * 2 + Vector3Int.forward * 2, BrickBlanks.OBrick);
            BricksCrashWrapper crashWrapper = new(_database);

            _databaseAccess.SetAndAddRecentControllableBrick(brick);
            _databaseAccess.SetAndAddRecentControllableBrick(brick2);

            Assert.AreEqual(1, _database.HeighestPoint);

            _databaseAccess.PlaceControllableBrick();
            crashWrapper.TryCrashAll();

            Assert.AreEqual(1, _database.HeighestPoint);
        }
    }
}