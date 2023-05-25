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
        /// Время за которое блок опускается
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// Начальная позиция блоков
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        [Space]

        [SerializeField] private GameObject _brickInput;
        [SerializeField] private BrickView _brickPrefab;
        [SerializeField] private Transform _worldPositionAnchor;

        /// <summary>
        /// Реализация для получения ввода от игрока
        /// </summary>
        private IBrickInputView _brickInputView;
        /// <summary>
        /// Смещение относительно мировых координат
        /// </summary>
        private Vector3 _worldPositionOffset;

        private float _lowerTimer;

        private BricksSpace _bricksSpace;
        private PlacingSurface _placingSurface;

        private IBrickFactory _brickFactory;
        private IBrickViewFactory _brickViewFactory;

        private BrickViewPresenter _brickPresenter;

        /// <summary>
        /// При запуске инициализирует начальный блок, фабрики, пространство блоков и так далее
        /// </summary>
        public override void Boot()
        {
            // Создание платформы для блоков
            _placingSurface = new(_surfaceSize, _worldPositionOffset);

            // Создание фабрики
            _brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            _brickViewFactory = new BrickViewFactory(_brickPrefab);

            // Создание начального блока из фабрики
            IBrick startBrick = _brickFactory.Create(_startBrickPosition);
            BrickView brickView = _brickViewFactory.Create(GetWorldPosition());

            // Создание пространства блоков
            _bricksSpace = new(_placingSurface, startBrick);

            // Подписка на нажатие кнопок
            _brickInputView.SetCallbacks();
            // Создание посредника между нажатием кнопок и реакции на это
            BrickInput brickInput = new(_bricksSpace, _brickInputView);
            // Обеспечение контранкта между нажатием кнопки и действием, то есть реакцией на нажатие
            brickInput.SetCallbacks();

            _brickPresenter = new(_bricksSpace, brickView);
            _brickPresenter.SetCallbacks();

            _bricksSpace.OnControllableBrickFall += CreateNewBlock;
        }

        /// <summary>
        /// Получить мировую позицию
        /// </summary>
        /// <returns></returns>
        private Vector3 GetWorldPosition()
        {
            return _placingSurface.GetWorldPosition(_startBrickPosition);
        }

        /// <summary>
        /// Создание нового блока
        /// </summary>
        private void CreateNewBlock()
        {
            IBrick newBrick = _brickFactory.Create(_startBrickPosition);
            BrickView newBrickView = _brickViewFactory.Create(GetWorldPosition());

            _bricksSpace.ChangeAndAddBlock(newBrick);
            _brickPresenter.Instance = newBrickView;
        }

        private void Update()
        {
            UpdateLowerTimerAndTryLower();
        }

        /// <summary>
        /// Обновляет таймер снижения и пробует опустить блок
        /// </summary>
        private void UpdateLowerTimerAndTryLower()
        {
            _lowerTimer += Time.deltaTime;

            if (_lowerTimer >= _lowerTick)
            {
                _bricksSpace.LowerBrickAndCheckGrounding();

                _lowerTimer = 0;
            }
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