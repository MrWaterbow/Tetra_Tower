using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public class BrickPattern
    {
        private static Vector3Int[] _value;

        public BrickPattern(Vector3Int[] value)
        {
            _value = value;
        }

        public static implicit operator BrickPattern(Vector3Int[] array)
        {
            return new BrickPattern(array);
        }

        public static implicit operator Vector3Int[](BrickPattern brickPattern)
        {
            return _value;
        }
    }
}