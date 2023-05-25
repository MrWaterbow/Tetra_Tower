using System.Collections.Generic;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BricksSpace
    {
        /// <summary>
        /// ¬се блоки, которые существуют в пространстве
        /// </summary>
        private readonly List<IBrick> _bricks;
        /// <summary>
        ///  онтролируемый игроком блок
        /// </summary>
        private readonly IBrick _controllableBrick;

        /// <summary>
        /// ѕоверхность на которую став€тс€ блоки
        /// </summary>
        private readonly PlacingSurface _surface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceSize">–азмер поверхности</param>
        /// <param name="worldPositionOffset">—мещение относительно мировых координат</param>
        /// <param name="controllableBrick"> онтролируемый блок</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, IBrick controllableBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<IBrick>() { controllableBrick };
            _controllableBrick = controllableBrick;
        }

        public BricksSpace(PlacingSurface surface, IBrick controllableBrick)
        {
            _surface = surface;

            _bricks = new List<IBrick>() { controllableBrick };
            _controllableBrick = controllableBrick;
        }

        /// <summary>
        /// Ёто свойство позвол€ет прочесть данные блока
        /// </summary>
        public IReadOnlyBrick ControllableBrick => _controllableBrick;

        /// <summary>
        /// ѕоверхность, на которую устанавливаютс€ блоки
        /// </summary>
        public PlacingSurface Surface => _surface;

        /// <summary>
        /// ћетод содержит проверку - можноли двинуть блок и в случае истины двигает его в указанном направлении
        /// </summary>
        /// <param name="direction">Ќаправление</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            if (_surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition))
            {
                _controllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// ¬озвращает будущую позицию относительно напрвлени€
        /// </summary>
        /// <param name="direction">Ќаправление</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_controllableBrick.Position.x + direction.x, _controllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// ћетод возвращает возможно ли двинуть блок в указанном направлении
        /// </summary>
        /// <param name="direction">Ќаправление</param>
        /// <returns></returns>
        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition);
        }

        /// <summary>
        /// ќпускает контролируемый блок
        /// </summary>
        public void LowerBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}