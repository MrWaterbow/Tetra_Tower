using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Поверхность на которую ставятся блоки.
    /// </summary>
    public readonly struct PlacingSurface
    {
        /// <summary>
        /// Размер площадки.
        /// </summary>
        public readonly HashSet<Vector2Int> SurfaceTiles;
        public readonly Vector2Int SurfaceSize;
        /// <summary>
        /// Для того что бы превращать локальные коорданаты блоков в мировые координаты относительно клиента. Он нужен что бы отобразить блок относительно мировых координат Unity со смещением относительно их.
        /// </summary>
        public readonly Vector3 WorldPositionOffset;

        /// <summary>
        /// Конструктор позволяет назначить размер площадки и смещение относительно мировых координат.
        /// </summary>
        /// <param name="surfaceSize">Размер площадки</param>
        /// <param name="worldPositionOffset">Смещение относительно мировых координат</param>
        public PlacingSurface(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            SurfaceTiles = new();
            SurfaceSize = surfaceSize;

            for (int x = 0; x < surfaceSize.x; x++)
            {
                for (int y = 0; y < surfaceSize.y; y++)
                {
                    SurfaceTiles.Add(new Vector2Int(x, y));
                }
            }

            WorldPositionOffset = worldPositionOffset;
        }

        /// <summary>
        /// Метод проверяет находится ли хотя бы одна клетка блока в рамках поверхности.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool PatternIntoSurfaceTiles(Vector3Int[] pattern, Vector2Int position)
        {
            foreach (Vector3Int tile in pattern)
            {
                Vector2Int featureCellPosition = new Vector2Int(tile.x, tile.z) + position;

                if (PositionIntoSurfaceTiles(featureCellPosition)) return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет находится ли позиция в рамках поверхности.
        /// </summary>
        /// <param name="position">Позиция, которую проверяет</param>
        /// <returns></returns>
        public bool PositionIntoSurfaceTiles(Vector2Int position)
        {
            return SurfaceTiles.Contains(position);
        }

        public bool PositionIntoSurfaceSize(Vector2Int position)
        {
            return position.x > -1 && position.x < SurfaceSize.x && position.y > -1 && position.y < SurfaceSize.y;
        }

        public void TryExtendSurface(IReadOnlyBrick brick)
        {
            foreach (Vector3Int tile in brick.Pattern)
            {
                Vector3Int tilePosition = tile + brick.Position;
                Vector2Int key = new(tilePosition.x, tilePosition.z);

                if (SurfaceTiles.Contains(key)) continue;

                SurfaceTiles.Add(key);
            }
        }
        
        /// <summary>
        /// Расчитывает и возвращает позицию относительно мировых координат.
        /// </summary>
        /// <param name="localPosition">Позиция в локальных координатах</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(Vector3Int localPosition)
        {
            return localPosition + WorldPositionOffset;
        }
    }
}