using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public static class BrickPatterns
    {
        public static readonly Vector3Int[] LBlock = new Vector3Int[4] 
        { 
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.left, 
            Vector3Int.left + Vector3Int.forward,
             
        };
    }

    public class Brick
    {
        private Vector3Int _position;
        private Vector3Int[] _pattern;

        public Brick(Vector3Int position, Vector3Int[] pattern)
        {
            _position = position;
            _pattern = pattern;
        }

        public Vector3Int Position => _position;
        public IReadOnlyCollection<Vector3Int> Pattern => _pattern;

        public void Move(Vector3Int direction)
        {
            _position += direction;
        }
    }
}