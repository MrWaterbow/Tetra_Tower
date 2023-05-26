using System;
using System.Collections.Generic;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BricksSpace
    {
        public event Action OnControllableBrickFall;
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
        private readonly PlacingSurface _surface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceSize">Размер платформы</param>
        /// <param name="worldPositionOffset">Смещение относительно мировых координат</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset)
        {
            _surface = new(surfaceSize, worldPositionOffset);
            _bricks = new List<Brick>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface">Платформа на которую ставяться блоки</param>
        /// <param name="controllableBrick">Контролирумый блок</param>
        public BricksSpace(PlacingSurface surface)
        {
            _surface = surface;
            _bricks = new List<Brick>();
        }

        /// <summary>
        /// Свойство, чтобы получать доступ к чтению данных из контролируемого блока
        /// </summary>
        public Brick ControllableBrick => _controllableBrick;

        /// <summary>
        /// Свойство для получения данных поверхности
        /// </summary>
        public PlacingSurface Surface => _surface;

        /// <summary>
        /// Проверяет возможность движения блока и в случае истины - двигает его в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            if(PossibleMoveBrickTo(direction))
            {
                _controllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// Проверяет возможность движения блока в указаном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition);
        }

        /// <summary>
        /// Расчитывает будущую позицию блока
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_controllableBrick.Position.x + direction.x, _controllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// Снижает высоту блока на одну единицу и проверяет находится ли он на земле
        /// </summary>
        public void LowerBrickAndCheckGrounding()
        {
            if(ControllableBrickOnGround() == false)
            {
                _controllableBrick.Move(Vector3Int.down);
            }

            if (ControllableBrickOnGround())
            {
                OnControllableBrickFall?.Invoke();
            }
        }

        /// <summary>
        /// Проверка блока находится ли он на земле
        /// </summary>
        /// <returns></returns>
        private bool ControllableBrickOnGround()
        {
            return _controllableBrick.Position.y == 0;
        }

        /// <summary>
        /// Меняет управляемый игроком блок и добавляет новый в список блоков
        /// </summary>
        /// <param name="brick"></param>
        public void ChangeAndAddRecentControllableBrick(Brick brick)
        {
            _controllableBrick = brick;
            _bricks.Add(brick);
        }
    }
}