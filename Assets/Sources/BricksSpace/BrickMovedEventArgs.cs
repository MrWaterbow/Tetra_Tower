﻿using System;

namespace Sources.BricksLogic
{
    public sealed class BrickMovedEventArgs : EventArgs
    {
        public readonly IBrick Brick;

        public BrickMovedEventArgs(IBrick brick)
        {
            Brick = brick;
        }
    }
}