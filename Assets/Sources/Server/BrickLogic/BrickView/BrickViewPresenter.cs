using Server.BricksLogic;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BrickViewPresenter
    {
        /// <summary>
        /// Обьект созданого блока
        /// </summary>
        public BrickView Instance;

        /// <summary>
        /// Пространство блоков
        /// </summary>
        private readonly BricksSpace _bricksSpace;

        public BrickViewPresenter(BricksSpace bricksSpace, BrickView instance)
        {
            Instance = instance;

            _bricksSpace = bricksSpace;
        }

        /// <summary>
        /// Подписывается на ивенты связанные с визуальным отображением блока
        /// </summary>
        public void SetCallbacks()
        {
            _bricksSpace.OnControllableBrickMoved += InvokeChangePosition;
        }

        /// <summary>
        /// Отписывается от ивентов связанных с визуальным отображением блока
        /// </summary>
        public void DisposeCallbacks()
        {
            _bricksSpace.OnControllableBrickMoved -= InvokeChangePosition;
        }

        /// <summary>
        /// Вызывает визуальное изменение позиции у созданого обьекта
        /// </summary>
        /// <param name="localPosition"></param>
        private void InvokeChangePosition(Vector3Int localPosition)
        {
            Instance.ChangePosition(_bricksSpace.Surface.GetWorldPosition(localPosition));
        }
    }
}