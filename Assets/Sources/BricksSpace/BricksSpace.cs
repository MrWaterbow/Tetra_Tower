using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public sealed class BricksSpace
    {
        //_bricks все блоки
        // _controllableBrick контролируемый игроком блок
        //_surface поверхность, на которую устанавливаютс€ блоки. „итает блоки встающие на неЄ, Ќапример, ѕровер€ет находитс€ ли блок на поверхности

        private readonly List<IBrick> _bricks;
        private readonly IBrick _controllableBrick;

        private readonly PlacingSurface _surface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceSize">–азмер поверхности</param>
        /// <param name="worldPositionOffset">—мещение относительно мировых координат</param>
        /// <param name="controlledBrick"> онтролируемый блок</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, IBrick controlledBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<IBrick>() { controlledBrick };
            _controllableBrick = controlledBrick;
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
        /// ¬озвращает будущую позицию относительно напрвлени€
        /// </summary>
        /// <param name="direction">Ќаправление</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_controllableBrick.Position.x + direction.x, _controllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// ќпускает контролируемый блок
        /// </summary>
        public void LowerControllableBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}