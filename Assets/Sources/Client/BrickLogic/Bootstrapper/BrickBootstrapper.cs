using Client.BootstrapperLogic;
using Server.BrickLogic;
using Server.Factories;
using UnityEngine;
using Zenject;

namespace Client.BrickLogic
{
    /// <summary>
    /// Запускает создание блоков и их визуальной составляющей.
    /// </summary>
    internal sealed class BrickBootstrapper : Bootstrapper, IBricksRuntimeData
    {
        /// <summary>
        /// Время за которое блок опускается.
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// Начальная позиция блоков.
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        /// <summary>
        /// Таймер до падения блока.
        /// </summary>
        private BrickView _currentBrickView;
        private float _lowerTimer;

        private IBrickViewFactory _brickViewFactory;
        private IControllableBrickViewPresenter _controllableViewPresenter;
        private IBrickFactory _brickFactory;
        private IReadOnlyBricksDatabase _database;
        private BrickMovementWrapper _brickMovementWrapper;
        private BricksDatabaseAccess _brickDatabaseAccess;
        private BricksCrashWrapper _bricksCrashWrapper;

        [Inject]
        private void Constructor(
            IBrickViewFactory brickViewFactory,
            IControllableBrickViewPresenter controllableViewPresenter,
            IBrickFactory brickFactory,
            IReadOnlyBricksDatabase database,
            BrickMovementWrapper brickMovementWrapper,
            BricksDatabaseAccess bricksDatabaseAccess,
            BricksCrashWrapper bricksCrashWrapper)
        {
            _brickViewFactory = brickViewFactory;
            _controllableViewPresenter = controllableViewPresenter;
            _brickFactory = brickFactory;
            _database = database;
            _brickMovementWrapper = brickMovementWrapper;
            _brickDatabaseAccess = bricksDatabaseAccess;
            _bricksCrashWrapper = bricksCrashWrapper;
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
            _brickMovementWrapper.OnControllableBrickFall += TestForCrash;
        }

        /// <summary>
        /// Создание нового блока и подключение его как управляем+ого 
        /// </summary>
        private void CreateAndSetControllableBrick()
        {
            Brick brick = _brickFactory.Create();

            _brickDatabaseAccess.SetAndAddRecentControllableBrick(brick);

            CreateBlockView();
        }

        private void TestForCrash()
        {
            _bricksCrashWrapper.TryCrashAll();
        }

        /// <summary>
        /// Создание нового отображения визуального блока
        /// </summary>
        /// <param name="brick"></param>
        private void CreateBlockView()
        {
            _currentBrickView?.DisposeCallbacks();
            _controllableViewPresenter?.DisposeCallbacks();

            _currentBrickView = _brickViewFactory.Create(_database.GetControllableBrickWorldPosition(), _database.ControllableBrick.Pattern);
            BrickViewPresenter brickPresenter = new(_database.ControllableBrick);
            _currentBrickView.SetCallbacks(_controllableViewPresenter, brickPresenter);
            brickPresenter.SetCallbacks();

            _controllableViewPresenter.SetAndInvokeCallbacks();
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