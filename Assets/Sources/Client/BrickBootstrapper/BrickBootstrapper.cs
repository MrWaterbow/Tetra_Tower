using Server.BricksLogic;
using Server.Factories;
using UnityEngine;

namespace Client.Bootstrapper
{
    internal class BrickBootstrapper : Bootstrapper
    {
        /// <summary>
        /// Размер поверхности для размещения блоков
        /// </summary>
        [SerializeField] private Vector2Int _surfaceSize;
        /// <summary>
        /// Начальная позиция блоков
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        [Space]

        [SerializeField] private GameObject _brickInput;
        [SerializeField] private Transform _worldPositionAnchor;

        /// <summary>
        /// Реализация для получения ввода от игрока
        /// </summary>
        private IBrickInputView _brickInputView;
        /// <summary>
        /// Смещение относительно мировых координат
        /// </summary>
        private Vector3 _worldPositionOffset;

        /// <summary>
        /// При запуске инициализирует начальный блок, фабрики, пространство блоков и так далее
        /// </summary>
        public override void Boot()
        {
            IBrickFactory brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            IBrick startBrick = brickFactory.Create(_startBrickPosition);

            PlacingSurface placingSurface = new(_surfaceSize, _worldPositionOffset);
            BricksSpace bricksSpace = new(placingSurface, startBrick);

            _brickInputView.SetCallbacks();
            BrickInput brickInput = new(bricksSpace, _brickInputView);
            brickInput.SetCallbacks();
        }

        private void OnValidate()
        {
            if(_brickInput != null && _brickInput.TryGetComponent<IBrickInputView>(out var component))
            {
                _brickInputView = component;
            }
            else
            {
                _brickInput = null;
            }

            if(_worldPositionAnchor != null)
            {
                _worldPositionOffset = _worldPositionAnchor.position;
            }
            else
            {
                _worldPositionOffset = Vector3.zero;
            }
        }
    }
}