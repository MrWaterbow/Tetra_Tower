using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksDatabase : IReadOnlyBricksDatabase
    {
        private readonly Dictionary<Vector2Int, int> _heightMap;

        /// <summary>
        /// Список со всеми блоками
        /// </summary>
        private readonly List<Brick> _bricks;

        /// <summary>
        /// Текущий контролируемый игроком блок
        /// </summary>
        public Brick ControllableBrick;

        /// <summary>
        /// Платформа на которую ставяться блоки
        /// </summary>
        public readonly PlacingSurface Surface;

        private BricksDatabase(Vector2Int surfaceSize)
        {
            _heightMap = new Dictionary<Vector2Int, int>();

            GenerateHeightMap(surfaceSize);

            _bricks = new List<Brick>();
            ControllableBrick = null;
        }

        private void GenerateHeightMap(Vector2Int surfaceSize)
        {
            for (int x = 0; x < surfaceSize.x; x++)
            {
                for (int y = 0; y < surfaceSize.y; y++)
                {
                    _heightMap.Add(new Vector2Int(x, y), 0);
                }
            }
        }

        public BricksDatabase(Vector2Int surfaceSize, Vector3 worldPositionOffset) : this(surfaceSize)
        {
            Surface = new(surfaceSize, worldPositionOffset);
        }

        public BricksDatabase(PlacingSurface placingSurface) : this(placingSurface.SurfaceSize)
        {
            Surface = placingSurface;
        }

        public IReadOnlyDictionary<Vector2Int, int> HeightMap => _heightMap;

        public IReadOnlyList<IReadOnlyBrick> Bricks => _bricks;

        IReadOnlyBrick IReadOnlyBricksDatabase.ControllableBrick => ControllableBrick;

        PlacingSurface IReadOnlyBricksDatabase.Surface => Surface;

        public void AddBrickAndUpdateHeightMap(Brick brick)
        {
            _bricks.Add(brick);

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                _heightMap[heightMapKey] = brick.Position.y + 1;
            }
        }

        public Vector3Int ComputeFeatureGroundPosition(Vector3Int direction)
        {
            Vector2Int heightMapKey = new(direction.x, direction.z);

            return new(direction.x, _heightMap[heightMapKey], direction.z);
        }
    }
}