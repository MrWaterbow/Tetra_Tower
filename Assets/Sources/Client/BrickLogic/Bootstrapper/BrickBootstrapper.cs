using Client.BootstrapperLogic;
using Server.BrickLogic;
using Server.Factories;
using UnityEngine;
using Zenject;

namespace Client.BrickLogic
{
    /// <summary>
    /// ��������� �������� ������ � �� ���������� ������������.
    /// </summary>
    internal sealed class BrickBootstrapper : Bootstrapper, IBricksRuntimeData
    {
        /// <summary>
        /// ����� �� ������� ���� ����������.
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// ��������� ������� ������.
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        /// <summary>
        /// ������ �� ������� �����.
        /// </summary>
        private BrickView _currentBrickView;
        private float _lowerTimer;

        private IBrickViewFactory _brickViewFactory;
        private IBrickFactory _brickFactory;
        private IReadOnlyBricksDatabase _database;
        private BrickMovementWrapper _brickMovementWrapper;
        private BricksDatabaseAccess _brickDatabaseAccess;
        private BricksCrashWrapper _bricksCrashWrapper;

        [Inject]
        private void Constructor(
            IBrickViewFactory brickViewFactory,
            IBrickFactory brickFactory,
            IReadOnlyBricksDatabase database,
            BrickMovementWrapper brickMovementWrapper,
            BricksDatabaseAccess bricksDatabaseAccess,
            BricksCrashWrapper bricksCrashWrapper)
        {
            _brickViewFactory = brickViewFactory;
            _brickFactory = brickFactory;
            _database = database;
            _brickMovementWrapper = brickMovementWrapper;
            _brickDatabaseAccess = bricksDatabaseAccess;
            _bricksCrashWrapper = bricksCrashWrapper;
        }

        public IReadOnlyBrickView CurrentBrickView => _currentBrickView;

        /// <summary>
        /// ��� ������� �������������� ��������� ����, �������, ������������ ������ � ��� �����
        /// </summary>
        public override void Boot()
        {
            // �������� �������������� �����
            CreateAndSetControllableBrick();

            // ����� �������� ������ ����� ��� ��� �������
            _brickMovementWrapper.OnControllableBrickFall += CreateAndSetControllableBrick;
            _brickMovementWrapper.OnControllableBrickFall += TestForCrash;
        }

        /// <summary>
        /// �������� ������ ����� � ����������� ��� ��� ���������+��� 
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
        /// �������� ������ ����������� ����������� �����
        /// </summary>
        /// <param name="brick"></param>
        private void CreateBlockView()
        {
            _currentBrickView?.DisposeCallbacks();

            _currentBrickView = _brickViewFactory.Create(_database.GetControllableBrickWorldPosition(), _database.ControllableBrick.Pattern);
            _currentBrickView.SetCallbacks(_database.ControllableBrick, _database.Surface);
        }

        private void Update()
        {
            UpdateLowerTimerAndTryLower();
        }

        /// <summary>
        /// ��������� ������ �������� � ������� �������� ����
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