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

        [SerializeField] private GameObject _brickInput;
        [SerializeField] private BrickView _brickPrefab;
        [SerializeField] private Transform _worldPositionAnchor;

        /// <summary>
        /// ���������� ��� ��������� ����� �� ������
        /// </summary>
        private IBrickInputView _brickInputView;
        /// <summary>
        /// �������� ������������ ������� ���������
        /// </summary>
        private Vector3 _worldPositionOffset;

        private float _lowerTimer;

        private BricksSpace _bricksSpace;
        private PlacingSurface _placingSurface;

        private IBrickFactory _brickFactory;
        private IBrickViewFactory _brickViewFactory;

        private BrickViewPresenter _brickPresenter;

        /// <summary>
        /// ��� ������� �������������� ��������� ����, �������, ������������ ������ � ��� �����
        /// </summary>
        public override void Boot()
        {
            // �������� ��������� ��� ������
            _placingSurface = new(_surfaceSize, _worldPositionOffset);

            // �������� �������
            _brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            _brickViewFactory = new BrickViewFactory(_brickPrefab);

            // �������� ���������� ����� �� �������
            IBrick startBrick = _brickFactory.Create(_startBrickPosition);
            BrickView brickView = _brickViewFactory.Create(GetWorldPosition());

            // �������� ������������ ������
            _bricksSpace = new(_placingSurface, startBrick);

            // �������� �� ������� ������
            _brickInputView.SetCallbacks();
            // �������� ���������� ����� �������� ������ � ������� �� ���
            BrickInput brickInput = new(_bricksSpace, _brickInputView);
            // ����������� ���������� ����� �������� ������ � ���������, �� ���� �������� �� �������
            brickInput.SetCallbacks();

            _brickPresenter = new(_bricksSpace, brickView);
            _brickPresenter.SetCallbacks();

            _bricksSpace.OnControllableBrickFall += CreateNewBlock;
        }

        /// <summary>
        /// �������� ������� �������
        /// </summary>
        /// <returns></returns>
        private Vector3 GetWorldPosition()
        {
            return _placingSurface.GetWorldPosition(_startBrickPosition);
        }

        /// <summary>
        /// �������� ������ �����
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