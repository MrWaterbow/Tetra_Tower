using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server.BrickLogic
{
    public sealed class BricksDatabase : IReadOnlyBricksDatabase
    {
        public event Action OnUpdateBricks;
        public event Action OnAddBrick;

        /// <summary>
        /// Карта блоков - это словарь, который хранит ссылку на блок по позиции.
        /// </summary>
        private readonly Dictionary<Vector3Int, Brick> _bricksMap;
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

        private BricksDatabase()
        {
            _bricksMap = new();
            _heightMap = new();

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

            UpdateHeightMap(brick);

            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;

                SetOrAddIntoBricksMap(tilePosition, brick);
            }

            Surface.TryExtendSurface(brick);

            OnAddBrick?.Invoke();
        }

        private void UpdateHeightMap(IReadOnlyBrick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int tileKeyPosition = new(tilePosition.x, tilePosition.z);

                if (GetHeightByKey(tileKeyPosition) == tilePosition.y)
                {
                    _heightMap[tileKeyPosition] = tilePosition.y + 1;
                }
            }
        }

        /// <summary>
        /// Пробует найти значение по ключу в карте блоков и в случае неудачи добавляет такой элемент
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
        public int GetHeightByBlock(IReadOnlyBrick brick)
        {
            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = tile + brick.Position;
                int i = 0;

                while (PatternOnGround(brick.Pattern, brick.Position - Vector3Int.up * i) == false)
                {
                    i++;
                }

                return tilePosition.y - i;
            }

            throw new Exception("Tiles not found");
        }

        /// <summary>
        /// Контролируемый блок на земле
        /// </summary>
        /// <returns></returns>
        public bool ControllableBrickOnGround()
        {
            return PatternOnGround(ControllableBrick.Pattern, ControllableBrick.Position);
        }

        /// <summary>
        /// Проверка блока находится ли он на земле.
        /// </summary>
        /// <returns></returns>
        public bool PatternOnGround(IReadOnlyCollection<Vector3Int> pattern, Vector3Int position)
        {
            bool onGround = false;

            foreach (Vector3Int tile in pattern)
            {
                Vector3Int tilePosition = tile + position;
                Vector3Int underPosition = tilePosition - Vector3Int.up;

                if(underPosition.y == -1)
                {
                    onGround = true;

                    break;
                }

                if (_bricksMap.TryGetValue(underPosition, out Brick value) == false)
                {
                    SetOrAddIntoBricksMap(underPosition, null);
                }

                if (_bricksMap[underPosition] != null)
                {
                    onGround = true;

                    break;
                }
            }

            return onGround;
        }

        /// <summary>
        /// Получить блок по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        public void RemoveTile(Vector3Int position)
        {
            Brick brick = GetBrickByKey(position);

            if (brick == null)
            {
                throw new NullReferenceException();
            }

            brick.RemoveTile(position);
            ClearBrickMap(position);
        }

        /// <summary>
        /// Разрушает блок с обновлением базы данных
        /// </summary>
        /// <param name="brick"></param>
        public void DestroyBrick(Brick brick)
        {
            _bricks.Remove(brick);
            ClearBricksMap(brick);
            ClearHeightMap(brick);
            ClearSurfaceExtend(brick);

            brick.Destroy();
        }

        /// <summary>
        /// Очищает карту блоков
        /// </summary>
        /// <param name="brick"></param>
        private void ClearBricksMap(IReadOnlyBrick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;

                _bricksMap.Remove(tilePosition);
            }
        }

        private void ClearHeightMap(IReadOnlyBrick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int tileKeyPosition = new(tilePosition.x, tilePosition.z);

                if (GetHeightByKey(tileKeyPosition) == tilePosition.y + 1)
                {
                    _heightMap[tileKeyPosition] = tilePosition.y - 1;
                }
            }
        }

        public int GetHeightByKey(Vector2Int key)
        {
            if (_heightMap.TryGetValue(key, out int value) == false)
            {
                return 0;
            }

            return _heightMap[key];
        }

        private void ClearSurfaceExtend(IReadOnlyBrick brick)
        {
            foreach (Vector3Int patternTile in brick.Pattern)
            {
                Vector3Int tilePosition = patternTile + brick.Position;
                Vector2Int tileKeyPosition = new(tilePosition.x, tilePosition.z);

                if (Surface.PositionIntoSurfaceSize(tileKeyPosition) == false)
                {
                    Surface.DeleteSurfaceTile(tileKeyPosition);
                }
            }
        }

        public void UpdateBricks()
        {
            OnUpdateBricks?.Invoke();
        }

        private void ClearBrickMap(Vector3Int position)
        {
            _bricksMap.Remove(position);
        }

        public LinkedList<IReadOnlyBrick> GetOnlyFirstUpperBricks(IReadOnlyBrick brick)
        {
            LinkedList<IReadOnlyBrick> upperBricks = new();

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = tile + brick.Position;
                Vector3Int upperPosition = tilePosition + Vector3Int.up;

                if(_bricksMap.TryGetValue(upperPosition, out Brick upperBrick))
                {
                    if (upperBrick == null) continue;

                    upperBricks.AddLast(upperBrick);
                }
            }

            return upperBricks;
        }

        /// <summary>
        /// Дает блоки соединенные с указанным сверху
        /// </summary>
        /// <param name="brick"></param>
        public LinkedList<IReadOnlyBrick> GetUpperBricks(IReadOnlyBrick brick)
        {
            LinkedList<IReadOnlyBrick> upperBricks = new();

            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = tile + brick.Position;
                Vector3Int upperPosition = tilePosition + Vector3Int.up;

                if(_bricksMap.TryGetValue(upperPosition, out Brick upperBrick))
                {
                    if (upperBrick == null) continue;

                    upperBricks.AddLast(upperBrick);

                    foreach (IReadOnlyBrick nextBrick in GetUpperBricks(upperBrick))
                    {
                        upperBricks.AddLast(nextBrick);
                    }
                }
            }

            return upperBricks;
        }

        /// <summary>
        /// Добавляет блок без обновления базы данных
        /// </summary>
        /// <param name="brick"></param>
        public void AddBrick(Brick brick)
        {
            _bricks.Add(brick);
        }

        public Vector3Int TryGetRandomBricksMapKey()
        {
            if (_bricksMap.Count == 0) throw new IndexOutOfRangeException();

            KeyValuePair<Vector3Int, Brick> pair = _bricksMap.ElementAt(UnityEngine.Random.Range(0, _bricksMap.Count));

            if(pair.Value == null)
            {
                return TryGetRandomBricksMapKey();
            }

            return pair.Key;
        }

        public int GetHeighestPoint()
        {
            int heighest = 0;

            foreach (int height in _heightMap.Values)
            {
                if (height > heighest)
                {
                    heighest = height;
                }
            }

            return heighest;
        }
    }
}