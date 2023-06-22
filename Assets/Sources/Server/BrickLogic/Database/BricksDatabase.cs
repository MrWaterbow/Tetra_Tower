using System.Collections.Generic;
using UnityEngine;
using static PlasticPipe.Server.MonitorStats;

namespace Server.BrickLogic
{
    public sealed class BricksDatabase : IReadOnlyBricksDatabase
    {
        /// <summary>
        /// Карта высот - это словарь, который хранит высоту по позиции на поверхности.
        /// </summary>
        private readonly Dictionary<Vector2Int, int> _heightMap;
        /// <summary>
        /// Карта блоков - это словарь, который хранит ссылку на блок по позиции.
        /// </summary>
        private readonly Dictionary<Vector3Int, Brick> _bricksMap;

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

        private BricksDatabase()
        {
            _heightMap = new();
            _bricksMap = new();

            _bricks = new();
            ControllableBrick = null;
        }


        public BricksDatabase(Vector2Int surfaceSize, Vector3 worldPositionOffset) : this()
        {
            Surface = new(surfaceSize, worldPositionOffset);
        }

        public BricksDatabase(PlacingSurface placingSurface) : this()
        {
            Surface = placingSurface;
        }

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
        public void AddBrickAndUpdateDatabase(Brick brick)
        {
            _bricks.Add(brick);

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);
                int height = tilePosition.y + 1;

                SetOrAddIntoHeightMap(heightMapKey, height);
                SetOrAddIntoBricksMap(tilePosition, brick);
            }
        }

        private void SetOrAddIntoHeightMap(Vector2Int key, int value)
        {
            try
            {
                _heightMap[key] = value;
            }
            catch
            {
                _heightMap.Add(key, value);
            }
        }

        private void SetOrAddIntoBricksMap(Vector3Int key, Brick value)
        {
            try
            {
                _bricksMap[key] = value;
            }
            catch
            {
                _bricksMap.Add(key, value);
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
                    height = GetHeightByKey(heightMapKey) - patternTile.y;
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
            if (_heightMap.TryGetValue(key, out int value) == false)
            {
                return 0;
            }

            return _heightMap[key];
        }

        public Brick GetBrickByKey(Vector3Int key)
        {
            if(_bricksMap.TryGetValue(key, out Brick value) == false)
            {
                return null;
            }

            return _bricksMap[key];
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

                if (_heightMap.TryGetValue(heightMapKey, out int value) == false)
                {
                    SetOrAddIntoHeightMap(heightMapKey, 0);
                }

                if (_heightMap[heightMapKey] == tilePosition.y)
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

        public void DestroyBrick(Brick brick)
        {
            _bricks.Remove(brick);
            ClearHeightMap(brick);
            ClearBricksMap(brick);

            brick.Destroy();
        }

        private void ClearHeightMap(Brick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int heightMapKey = new(tilePosition.x, tilePosition.z);
                int underHeight = 0;
                int i = 1;

                while(GetBrickByKey(tilePosition - Vector3Int.up * i) == null && tilePosition.y - (i - 1) > 0)
                {
                    underHeight++;
                    i++;
                }

                _heightMap[heightMapKey] = tilePosition.y - underHeight;
            }
        }

        private void ClearBricksMap(Brick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;

                _bricksMap.Remove(tilePosition);
            }
        }

        public void AddBrick(Brick brick)
        {
            _bricks.Add(brick);
        }
    }
}