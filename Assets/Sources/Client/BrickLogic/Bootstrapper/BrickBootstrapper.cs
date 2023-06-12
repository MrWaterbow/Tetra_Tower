using Client.BootstrapperLogic;
using Server.BrickLogic;
using Server.Factories;
using UnityEngine;
using Zenject;

namespace Client.BrickLogic
{
    internal sealed class BrickBootstrapper : Bootstrapper, IBricksRuntimeData
    {
        /// <summary>
        /// Время за которое блок опускается
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// Начальная позиция блоков
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        /// <summary>
        /// Таймер до падения блока
        /// </summary>
        private BrickView _currentBrickView;
        private float _lowerTimer;

        private IBrickViewFactory _brickViewFactory;
        private IBrickViewPresenter _brickViewPresenter;
        private IBrickFactory _brickFactory;
        private BrickMovementWrapper _brickMovementWrapper;
        private BricksDatabaseAccess _brickDatabaseAccess;

        [Inject]
        private void Constructor(
            IBrickViewFactory brickViewFactory,
            IBrickViewPresenter brickPresenter,
            IBrickFactory brickFactory,
            BrickMovementWrapper brickMovementWrapper,
            BricksDatabaseAccess bricksDatabaseAccess)
        {
            _brickViewFactory = brickViewFactory;
            _brickViewPresenter = brickPresenter;
            _brickFactory = brickFactory;
            _brickMovementWrapper = brickMovementWrapper;
            _brickDatabaseAccess = bricksDatabaseAccess;
        }

        public IReadOnlyBrickView CurrentBrickView => _currentBrickView;

        /// <summary>
        /// При запуске инициализирует начальный блок, фабрики, пространство блоков и так далее
        /// </summary>
        public override void Boot()
        {
            // Создание контролирумого блока
            CreateAndSetControllableBrick();

            // Вызов создание нового блока при его падении
            _brickMovementWrapper.OnControllableBrickFall += CreateAndSetControllableBrick;
        }

        /// <summary>
        /// Создание нового блока и подключение его как управляемого 
        /// </summary>
        private void CreateAndSetControllableBrick()
        {
            Brick brick = _brickFactory.Create(_startBrickPosition);

            _brickDatabaseAccess.ChangeAndAddRecentControllableBrick(brick);

            CreateBlockView();
        }

        /// <summary>
        /// Создание нового отображения визуального блока
        /// </summary>
        /// <param name="brick"></param>
        private void CreateBlockView()
        {
            _currentBrickView?.DisposeCallbacks();
            _brickViewPresenter?.DisposeCallbacks();

            _currentBrickView = _brickViewFactory.Create(_brickMovementWrapper.GetControllableBrickWorldPosition());
            _currentBrickView.SetCallbacks(_brickViewPresenter);

            _brickViewPresenter.SetAndInvokeCallbacks();
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
                _brickMovementWrapper.LowerBrickAndCheckGrounding();

                _lowerTimer = 0;
            }
        }
    }
}