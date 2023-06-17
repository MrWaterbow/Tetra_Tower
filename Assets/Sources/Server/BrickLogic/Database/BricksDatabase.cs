using System.Collections.Generic;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksDatabase : IReadOnlyBricksDatabase
    {
        /// <summary>
        /// Карта высот - это словарь, который хранит высоту по позиции на поверхности.
        /// </summary>
        private readonly Dictionary<Vector2Int, int> _heightMap;

        /// <summary>
        /// Список со всеми блоками.
        /// </summary>
        private readonly List<Brick> _bricks;

        /// <summary>
        /// Текущий контролируемый игроком блок.
        /// </summary>
        public Brick ControllableBrick;

        /// <summary>
        /// Платформа на которую ставяться блоки.
        /// </summary>
        public readonly PlacingSurface Surface;

        private BricksDatabase(Vector2Int surfaceSize)
        {
            _heightMap = new Dictionary<Vector2Int, int>();

            GenerateHeightMap(surfaceSize);

            _bricks = new List<Brick>();
            ControllableBrick = null;
        }

        /// <summary>
        /// Генерирует ключи для поверхности с требуемым смещением.
        /// </summary>
        /// <param name="surfaceSize"></param>
        private void GenerateHeightMap(Vector2Int surfaceSize)
        {
            for (int x = -2; x < surfaceSize.x + 2; x++)
            {
                for (int y = -2; y < surfaceSize.y + 2; y++)
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

        /// <summary>
        /// Доступ к чтению карты высот.
        /// </summary>
        public IReadOnlyDictionary<Vector2Int, int> HeightMap => _heightMap;
        /// <summary>
        /// Доступ к чтению поставленных блоков.
        /// </summary>
        public IReadOnlyList<IReadOnlyBrick> Bricks => _bricks;
        /// <summary>
        /// Доступ к чтению данных из текущего контролируемого блока.
        /// </summary>
        IReadOnlyBrick IReadOnlyBricksDatabase.ControllableBrick => ControllableBrick;
        /// <summary>
        /// Возвращает копию поверхности на которую ставят блоки.
        /// </summary>
        PlacingSurface IReadOnlyBricksDatabase.Surface => Surface;

        /// <summary>
        /// Добавляет блок в список поставленных и обновляет карту высот.
        /// </summary>
        /// <param name="brick"></param>
        public void AddBrickAndUpdateHeightMap(Brick brick)
        {
            _bricks.Add(brick);

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                _heightMap[heightMapKey] = tilePosition.y + 1;
            }
        }

        /// <summary>
        /// Возвращает высоту по паттерну блока.
        /// </summary>
        /// <param name="brick"></param>
        /// <returns></returns>
        public int GetHeightByPattern(IReadOnlyBrick brick)
        {
            int height = 0;

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (GetHeightByKey(heightMapKey) > height)
                {
                    height = HeightMap[heightMapKey];
                }
            }

            return height;
        }

        /// <summary>
        /// Возвращает высоту по ключу (позиции).
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetHeightByKey(Vector2Int key)
        {
            return _heightMap[key];
        }

        /// <summary>
        /// Возвращает мировую позицию контролирумого блока.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetControllableBrickWorldPosition()
        {
            return Surface.GetWorldPosition(ControllableBrick.Position);
        }

        /// <summary>
        /// Проверка блока находится ли он на земле.
        /// </summary>
        /// <returns></returns>
        public bool ControllableBrickOnGround()
        {
            bool onGround = false;

            foreach (Vector3Int patternTile in ControllableBrick.Pattern)
            {
                Vector3Int tilePosition = patternTile + ControllableBrick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);

                if (HeightMap[heightMapKey] == tilePosition.y)
                {
                    onGround = true;
                }
            }

            return onGround;
        }

        /// <summary>
        /// Возвращает наивысшую высоту.
        /// </summary>
        /// <returns></returns>
        public int GetHeighestPoint()
        {
            int heighestPoint = 0;

            foreach (KeyValuePair<Vector2Int, int> pair in _heightMap)
            {
                if (pair.Value > heighestPoint)
                {
                    heighestPoint = pair.Value;
                }
            }

            return heighestPoint;
        }
    }
}