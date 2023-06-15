using System;

namespace Server.BrickLogic
{
    /// <summary>
    /// Исключение выбрасывается при попытке опустить блок, который уже находится на земле.
    /// </summary>
    public sealed class BrickOnGroundException : Exception
    {
    }
}