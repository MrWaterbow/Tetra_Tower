namespace Server.BrickLogic
{
    /// <summary>
    /// Модуль для безопасного изменения значений в базе данных.
    /// </summary>
    public sealed class BricksDatabaseAccess
    {
        /// <summary>
        /// База данных
        /// </summary>
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

        /// <summary>
        /// Добавляет контролируемый блок в список поставленных блоков и обнуляет его
        /// </summary>
        public void PlaceControllableBrick()
        {
            _database.AddBrickAndUpdateHeightMap(_database.ControllableBrick);

            _database.ControllableBrick = null;
        }
    }
}