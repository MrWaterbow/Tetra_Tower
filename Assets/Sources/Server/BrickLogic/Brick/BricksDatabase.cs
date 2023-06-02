using System.Collections.Generic;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IReadOnlyBricksDatabase
    {
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }
        IReadOnlyBrick ControllableBrick { get; }
    }

    public class BricksDatabase
    {
        /// <summary>
        /// Список со всеми блоками
        /// </summary>
        private readonly List<Brick> _bricks;

        /// <summary>
        /// Текущий контролируемый игроком блок
        /// </summary>
        private Brick _controllableBrick;

        /// <summary>
        /// Платформа на которую ставяться блоки
        /// </summary>
        public readonly PlacingSurface Surface;

        public BricksDatabase(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            _bricks = new List<Brick>();
            _controllableBricks = null;

            Surface = new(surfaceSize, worldPositionOffset);
        }

        public BricksDatabase(PlacingSurface placingSurface)
        {
            _bricks = new List<Brick>();
            _controllableBricks = null;

            Surface = placingSurface;
        }

        /// <summary>
        /// Проверяет возможность движения блока в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return Surface.PatternInSurfaceLimits(_controllableBricks.Pattern, featurePosition);
        }

        public Vector3Int ComputeFeatureGroundPosition(Vector3Int direction)
        {
            return new(direction.x, 0, direction.z);
        }

        /// <summary>
        /// Расчитывает будущую позицию блока
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_controllableBricks.Position.x + direction.x, _controllableBricks.Position.z + direction.z);
        }

        /// <summary>
        /// Проверка блока находится ли он на земле
        /// </summary>
        /// <returns></returns>
        public bool ControllableBrickOnGround()
        {
            return _controllableBricks.Position.y == 0;
        }
    }
}