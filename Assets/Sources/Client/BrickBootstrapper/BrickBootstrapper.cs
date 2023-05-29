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
        /// ������ ����������� ��� ���������� ������
        /// </summary>
        [SerializeField] private Vector2Int _surfaceSize;
        /// <summary>
        /// ����� �� ������� ���� ����������
        /// </summary>
        [SerializeField] private float _lowerTick;
        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        [SerializeField] private Vector3Int _startBrickPosition;

        [Space]

        [SerializeField] private GameObject _brickInputObject;
        [SerializeField] private BrickView _brickPrefab;
        [SerializeField] private Transform _worldPositionAnchor;

        /// <summary>
        /// ���������� ��������� ����� �� ������
        /// </summary>
        private IBrickInputView _brickInput;
        /// <summary>
        /// �������� ������������ ������� ���������
        /// </summary>
        private Vector3 _worldPositionOffset;
        /// <summary>
        /// ������ �� ������� �����
        /// </summary>
        private float _lowerTimer;

        private BricksSpace _bricksSpace;

        private BrickView _currentBrickView;

        private IBrickFactory _brickFactory;
        private IBrickViewFactory _brickViewFactory;

        private IBrickViewPresenter _brickPresenter;
        private IBrickInputPresenter _brickInputPresenter;

        /// <summary>
        /// ��� ������� �������������� ��������� ����, �������, ������������ ������ � ��� �����
        /// </summary>
        public override void Boot()
        {
            // �������� �������
            _brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            _brickViewFactory = new BrickViewFactory(_brickPrefab);

            // �������� ������������ ������
            _bricksSpace = new(_surfaceSize, _worldPositionOffset);

            // �������� ����������
            _brickPresenter = new BrickViewPresenter(_bricksSpace);

            // �������� �������������� �����
            CreateAndSetControllableBrick();

            // �������� ���������� ��� ������ ����������
            _brickInputPresenter = new BrickInputPresenter(_bricksSpace);
            _brickInput.Presenter = _brickInputPresenter;

            // ����� �������� ������ ����� ��� ��� �������
            _bricksSpace.OnControllableBrickFall += CreateAndSetControllableBrick;
        }

        /// <summary>
        /// �������� ������ ����� � ����������� ��� ��� ������������ 
        /// </summary>
        private void CreateAndSetControllableBrick()
        {
            Brick brick = _brickFactory.Create(_startBrickPosition);

            _bricksSpace.ChangeAndAddRecentControllableBrick(brick);

            CreateBlockView();
        }

        /// <summary>
        /// �������� ������ ����������� ����������� �����
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
        /// �������� ������� �������
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
        /// ��������� ������ �������� � ������� �������� ����
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