using NUnit.Framework;
using Server.AsteroidLogic;
using Server.BrickLogic;
using System.Linq;
using UnityEngine;

namespace Tests
{
    public sealed class AsteroidTests
    {
        private BricksDatabase _bricksDatabase;
        private AsteroidsDatabase _asteroidsDatabase;

        private AsteroidWrapper _asteroidWrapper;
        private BricksDatabaseAccess _bricksAccess;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            IAsteroidFactory factory = new AsteroidFactory(new[] { Vector3Int.zero }, 3);
            _bricksDatabase = new(surface);
            _asteroidsDatabase = new(_bricksDatabase, factory);

            _asteroidWrapper = new(_asteroidsDatabase);
            _bricksAccess = new(_bricksDatabase);
        }

        [Test]
        public void RemoveTileTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBrick);

            _bricksAccess.SetAndAddRecentControllableBrick(brick);
            _bricksAccess.PlaceControllableBrick();

            _bricksDatabase.RemoveTile(Vector3Int.zero);

            Assert.AreEqual(3, brick.Pattern.Count);

            Assert.IsFalse(brick.Pattern.Contains(Vector3Int.zero));
            Assert.IsTrue(brick.Pattern.Contains(Vector3Int.right));
            Assert.IsTrue(brick.Pattern.Contains(Vector3Int.forward));
            Assert.IsTrue(brick.Pattern.Contains(Vector3Int.right + Vector3Int.forward));
        }
    }
}