using NUnit.Framework;
using Server.BrickLogic;
using UnityEngine;

namespace Tests
{
    public sealed class FootFactorCalculatingTests
    {
        private BricksCrashWrapper _crashWrapper;
        private BricksDatabase _database;

        [SetUp]
        public void Setup()
        {
            PlacingSurface surface = new(Vector2Int.one * 3, Vector3.zero);
            _database = new BricksDatabase(surface);

            _crashWrapper = new(_database);
        }

        [Test]
        public void CalculateFootFactorByGroundWithoutBricksTest()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.OBlock.BrickPattern);

            Assert.AreEqual(4f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.left);

            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.back * 2);

            Assert.AreEqual(-4f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.up);

            Assert.AreEqual(-4f, _crashWrapper.ComputeFootFactor(brick));
        }

        [Test]
        public void CalculateFootFactorWithLBlock()
        {
            Brick brick = new(Vector3Int.zero, BrickBlanks.LBlock.BrickPattern);

            Assert.AreEqual(0.585786343f, _crashWrapper.ComputeFootFactor(brick));

            brick.ChangePosition(Vector3Int.left);

            Assert.AreEqual(-4.23606777f, _crashWrapper.ComputeFootFactor(brick));
        }

        [Test]
        public void CalculateFootFactorWithBricksTest()
        {
            Brick groundBrick = new(Vector3Int.zero, BrickBlanks.OBlock.BrickPattern);
            Brick groundBrick2 = new(Vector3Int.right * 2, BrickBlanks.OBlock.BrickPattern);
            Brick mainBrick = new(Vector3Int.forward + Vector3Int.up, BrickBlanks.OBlock.BrickPattern);

            _database.AddBrickAndUpdateDatabase(groundBrick);
            _database.AddBrickAndUpdateDatabase(groundBrick2);

            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(mainBrick));

            mainBrick.ChangePosition(Vector3Int.forward + Vector3Int.right * 3 + Vector3Int.up);

            Assert.AreEqual(-1 - Mathf.Sqrt(2), _crashWrapper.ComputeFootFactor(mainBrick));
        }

        [Test]
        public void UnstableEffectStoppingOnSupportTest()
        {
            Brick unstableBrick = new(Vector3Int.left, BrickBlanks.OBlock.BrickPattern);
            Brick supportingBrick = new(Vector3Int.up, BrickBlanks.OBlock.BrickPattern);

            _database.AddBrickAndUpdateDatabase(unstableBrick);
            _database.AddBrickAndUpdateDatabase(supportingBrick);

            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(unstableBrick));
            Assert.AreEqual(0f, _crashWrapper.ComputeFootFactor(supportingBrick));

            Brick helpingToSupportBrick = new(Vector3Int.right, BrickBlanks.OBlock.BrickPattern);

            _database.AddBrickAndUpdateDatabase(helpingToSupportBrick);

            Assert.AreEqual(4f, _crashWrapper.ComputeFootFactor(supportingBrick));
            Assert.AreEqual(2f, _crashWrapper.ComputeFootFactor(unstableBrick));

            Brick helpingToSupportBrick2 = new(Vector3Int.up * 2, BrickBlanks.OBlock.BrickPattern);

            _database.AddBrickAndUpdateDatabase(helpingToSupportBrick2);

            Assert.AreEqual(4f, _crashWrapper.ComputeFootFactor(unstableBrick));
        }

        [Test]
        public void DestroyBrickOnNegativeSupportTest()
        {
            Brick destroyingBrick = new(Vector3Int.left, BrickBlanks.OBlock.BrickPattern);
            Brick negativeBrick = new(Vector3Int.left * 2 + Vector3Int.up, BrickBlanks.OBlock.BrickPattern);

            _database.AddBrickAndUpdateDatabase(destroyingBrick);
            _database.AddBrickAndUpdateDatabase(negativeBrick);

            Assert.AreEqual(-2f, _crashWrapper.ComputeFootFactor(destroyingBrick));
        }
    }
}