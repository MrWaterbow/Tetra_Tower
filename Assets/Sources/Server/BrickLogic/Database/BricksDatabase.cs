using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public class BricksDatabase
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

        /// <summary>
        /// Проверяет возможность движения блока в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return Surface.PatternInSurfaceLimits(ControllableBrick.Pattern, featurePosition);
        }

        /// <summary>
        /// Расчитывает будущую позицию блока
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(ControllableBrick.Position.x + direction.x, ControllableBrick.Position.z + direction.z);
        }

        public Vector3Int ComputeFeatureGroundPosition(Vector3Int direction)
        {
            Vector2Int heightMapKey = new(direction.x, direction.z);

            return new(direction.x, _heightMap[heightMapKey], direction.z);
        }

        /// <summary>
        /// Проверка блока находится ли он на земле
        /// </summary>
        /// <returns></returns>
        public bool ControllableBrickOnGround()
        {
            bool onGround = false;

            foreach (Vector3Int patternTile in ControllableBrick.Pattern)
            {
                Vector3Int tilePosition = patternTile + ControllableBrick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (_heightMap[heightMapKey] == tilePosition.y - 1)
                {
                    onGround = true;
                }
            }

            return onGround;
        }

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

        public int GetHeightByPattern(Brick brick)
        {
            int height = 0;

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (_heightMap[heightMapKey] > height)
                {
                    height = _heightMap[heightMapKey] + 1;
                }
            }

            return height;
        }

        public int GetHeightByKey(Vector2Int heightMapKey)
        {
            return _heightMap[heightMapKey];
        }
    }
}