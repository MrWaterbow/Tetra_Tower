namespace Server.BrickLogic
{
    public sealed class BricksRotatingWrapper
    {
        private readonly BricksDatabase _database;

        public BricksRotatingWrapper(BricksDatabase database)
        {
            _database = database;
        }

        public void TryRotate90()
        {
            _database.ControllableBrick.Rotate90();
        }

        public void TryRotateMinus90()
        {
            _database.ControllableBrick.RotateMinus90();
        }
    }
}