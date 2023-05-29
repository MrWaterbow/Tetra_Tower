using Client.BricksLogic;
using Client.Factories;
using Client.Input;
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

        [SerializeField] private GameObject _brickInputObject;
        [SerializeField] private BrickView _brickPrefab;
        [SerializeField] private Transform _worldPositionAnchor;

        /// <summary>
        /// Реализация получения ввода от игрока
        /// </summary>
        private IBrickInputView _brickInput;
        /// <summary>
        /// Смещение относительно мировых координат
        /// </summary>
        private Vector3 _worldPositionOffset;
        /// <summary>
        /// Таймер до падения блока
        /// </summary>
        private float _lowerTimer;

        private BricksSpace _bricksSpace;

        private BrickView _currentBrickView;

        private IBrickFactory _brickFactory;
        private IBrickViewFactory _brickViewFactory;

        private IBrickViewPresenter _brickPresenter;
        private IBrickInputPresenter _brickInputPresenter;

        /// <summary>
        /// При запуске инициализирует начальный блок, фабрики, пространство блоков и так далее
        /// </summary>
        public override void Boot()
        {
            // Создание фабрики
            _brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            _brickViewFactory = new BrickViewFactory(_brickPrefab);

            // Создание пространства блоков
            _bricksSpace = new(_surfaceSize, _worldPositionOffset);

            // Создание презентера
            _brickPresenter = new BrickViewPresenter(_bricksSpace);

            // Создание контролирумого блока
            CreateAndSetControllableBrick();

            // Создание презентера для кнопок управления
            _brickInputPresenter = new BrickInputPresenter(_bricksSpace);
            _brickInput.Presenter = _brickInputPresenter;

            // Вызов создание нового блока при его падении
            _bricksSpace.OnControllableBrickFall += CreateAndSetControllableBrick;
        }

        /// <summary>
        /// Создание нового блока и подключение его как управляемого 
        /// </summary>
        private void CreateAndSetControllableBrick()
        {
            Brick brick = _brickFactory.Create(_startBrickPosition);

            _bricksSpace.ChangeAndAddRecentControllableBrick(brick);

            CreateBlockView();
        }

        /// <summary>
        /// Создание нового отображения визуального блока
        /// </summary>
        /// <param name="brick"></param>
        private void CreateBlockView()
        {
            _currentBrickView?.DisposeCallbacks();
            _brickPresenter?.DisposeCallbacks();

            _brickPresenter.SetCallbacks();

            _currentBrickView = _brickViewFactory.Create(GetWorldPosition());
            _currentBrickView.SetCallbacks(_brickPresenter);
        }

        /// <summary>
        /// Получить мировую позицию
        /// </summary>
        /// <returns></returns>
        private Vector3 GetWorldPosition()
        {
            return _placingSurface.GetWorldPosition(_startBrickPosition);
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
            if(_brickInputObject != null && _brickInputObject.TryGetComponent(out IBrickInputView component))
            {
                _brickInput = component;
            }
            else
            {
                _brickInputObject = null;
            }

            if (_worldPositionAnchor != null)
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