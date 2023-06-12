namespace Server.BrickLogic
{
    public sealed class BricksDatabaseAccess
    {
        private readonly BricksDatabase _database;

        public BricksDatabaseAccess(BricksDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Меняет управляемый игроком блок и добавляет новый в список блоков
        /// </summary>
        /// <param name="brick"></param>
        public void ChangeAndAddRecentControllableBrick(Brick brick)
        {
            if (_database.ControllableBrick != null)
            {
                _database.AddBrickAndUpdateHeightMap(_database.ControllableBrick);
            }

            _database.ControllableBrick = brick;
        }

        public void PlaceControllableBrick()
        {
            _database.AddBrickAndUpdateHeightMap(_database.ControllableBrick);

            _database.ControllableBrick = null;
        }
    }
}