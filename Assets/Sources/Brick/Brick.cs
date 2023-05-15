using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public class Brick : IBrick
    {
        private Vector3Int _position;
        private BrickPattern _pattern;

        public Brick(Vector3Int position, BrickPattern pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        public Vector3Int Position => _position;
        public Vector3Int[] Pattern => _pattern;

        public void Move(Vector3Int direction)
        {
            _position += direction;
        }
    }
}