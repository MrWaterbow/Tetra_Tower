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
        /// ����� �� ������� ���� ����������
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        /// <summary>
        /// ������ �� ������� �����
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
        /// ��� ������� �������������� ��������� ����, �������, ������������ ������ � ��� �����
        /// </summary>
        public override void Boot()
        {
            // �������� �������������� �����
            CreateAndSetControllableBrick();

            // ����� �������� ������ ����� ��� ��� �������
            _brickMovementWrapper.OnControllableBrickFall += CreateAndSetControllableBrick;
        }

        /// <summary>
        /// �������� ������ ����� � ����������� ��� ��� ������������ 
        /// </summary>
        private void CreateAndSetControllableBrick()
        {
            Brick brick = _brickFactory.Create(_startBrickPosition);

            _brickDatabaseAccess.ChangeAndAddRecentControllableBrick(brick);

            CreateBlockView();
        }

        /// <summary>
        /// �������� ������ ����������� ����������� �����
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