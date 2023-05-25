using UnityEngine;

namespace Server.BricksLogic
{
    public readonly struct PlacingSurface
    {
        /// <summary>
        /// Размер площадки
        /// </summary>
        public readonly Vector2Int SurfaceSize;
        /// <summary>
        /// Для того что бы превращать локальные коорданаты блоков в мировые координаты относительно клиента. Он нужен что бы отобразить блок относительно мировых координат Unity со смещением относительно их.
        /// </summary>
        public readonly Vector3 WorldPositionOffset;

        /// <summary>
        /// Конструктор позволяет назначить размер площадки и смещение относительно мировых координат
        /// </summary>
        /// <param name="surfaceSize">Размер площадки</param>
        /// <param name="worldPositionOffset">Смещение относительно мировых координат</param>
        public PlacingSurface(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            SurfaceSize = surfaceSize;
            WorldPositionOffset = worldPositionOffset;
        }

        /// <summary>
        /// Метод проверяет находится ли хотя бы одна клетка блока в рамках поверхности
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool PatternInSurfaceLimits(Vector3Int[] pattern, Vector2Int position)
        {
            foreach (Vector3Int cell in pattern)
            {
                Vector2Int featureCellPosition = new Vector2Int(cell.x, cell.z) + position;

                if (PositionInSurfaceLimits(featureCellPosition)) return true;
            }

            return false;
        }
        /// <summary>
        /// Проверяет находится ли позиция в рамках поверхности
        /// </summary>
        /// <param name="position">Позиция, которую проверяет</param>
        /// <returns></returns>
        public bool PositionInSurfaceLimits(Vector2Int position)
        {
            return position.x > -1 && position.x < SurfaceSize.x && position.y > -1 && position.y < SurfaceSize.y;
        }

        /// <summary>
        /// Расчитывает и возвращает позицию относительно мировых координат
        /// </summary>
        /// <param name="localPosition">Позиция в локальных координатах</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(Vector3Int localPosition)
        {
            return localPosition + WorldPositionOffset;
        }
    }
}