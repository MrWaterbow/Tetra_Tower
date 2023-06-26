namespace Server.BrickLogic
{
    public static class BricksMatrixRotationBlanks
    {
        public static readonly int[,] LBlock0DegressRotatedMatrix = new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };

        public static readonly int[,] LBlock90DegressRotatedMatrix = new int[,]
        {
            { 0, 1, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };

        public static readonly int[,] LBlock180DegressRotatedMatrix = new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 1 }
        };

        public static readonly int[,] LBlock270DegressRotatedMatrix = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        };
    }
}