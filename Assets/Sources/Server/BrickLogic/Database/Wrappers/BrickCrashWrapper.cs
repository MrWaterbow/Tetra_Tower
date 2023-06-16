namespace Server.BrickLogic
{
    public sealed class BrickCrashWrapper
    {
        private readonly BricksDatabase _database;

        public BrickCrashWrapper(BricksDatabase database)
        {
            _database = database;
        }
    }
}