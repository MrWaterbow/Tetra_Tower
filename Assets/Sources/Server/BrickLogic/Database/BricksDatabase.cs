using System.Collections.Generic;
using UnityEngine;

namespace Server.BricksLogic
{
    public interface IReadOnlyBricksDatabase
    {
        IReadOnlyList<IReadOnlyBrick> Bricks { get; }
        IReadOnlyBrick ControllableBrick { get; }

        IReadOnlyPlacingSurface Surface { get; }
    }

    public class BricksDatabase : IReadOnlyBricksDatabase
    {
        /// <summary>
        /// Список со всеми блоками
        /// </summary>
        public readonly List<Brick> Bricks;

        /// <summary>
        /// Текущий контролируемый игроком блок
        /// </summary>
        public Brick ControllableBrick;

        /// <summary>
        /// Платформа на которую ставяться блоки
        /// </summary>
        public readonly PlacingSurface Surface;

        public BricksDatabase(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            Bricks = new List<Brick>();
            ControllableBrick = null;

            Surface = new(surfaceSize, worldPositionOffset);
        }

        public BricksDatabase(PlacingSurface placingSurface)
        {
            Bricks = new List<Brick>();
            ControllableBrick = null;

            Surface = placingSurface;
        }

        IReadOnlyList<IReadOnlyBrick> IReadOnlyBricksDatabase.Bricks => Bricks;

        IReadOnlyBrick IReadOnlyBricksDatabase.ControllableBrick => ControllableBrick;

        IReadOnlyPlacingSurface IReadOnlyBricksDatabase.Surface => Surface;

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
            return new(ControllableBrick.Position.x + direction.x, ControllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// Проверка блока находится ли он на земле
        /// </summary>
        /// <returns></returns>
        public bool ControllableBrickOnGround()
        {
            return ControllableBrick.Position.y == 0;
        }
    }
}