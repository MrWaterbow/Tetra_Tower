using Sources.BlockLogic;
using Sources.Factories;
using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInstaller : MonoInstaller
    {
        [SerializeField] private int _height;
        [SerializeField] private float _fallTick;

        private IBuildingInput _input;
        private BlockFactory _blockFactory;

        private IBlock _currentBlock;

        private float _tick;

        [Inject]
        private void Construct(BlockFactory blockFactory, IBuildingInput input)
        {
            _blockFactory = blockFactory;
            _input = input;
        }

        private void Update()
        {
            _tick += Time.deltaTime;

            if (_tick >= _fallTick)
            {
                _currentBlock.Fall();

                _tick = 0;
            }
        }

        private void OnEnable()
        {
            _input.MovingUp += MovingUp;
        }

        private void OnDisable()
        {
            _input.MovingUp -= MovingUp;
        }

        private void OnValidate()
        {
            _height = Mathf.Clamp(_height, 0, int.MaxValue);
            _fallTick = Mathf.Clamp(_fallTick, 0, float.MaxValue);
        }

        public override void InstallBindings()
        {
            _currentBlock = _blockFactory.Create(BlockType.Start, _height);
        }

        private void MovingUp()
        {
            _currentBlock.Move(Vector3.right);
        }

        private void MovingDown()
        {

        }

        private void MovingRight()
        {

        }

        private void MovingLeft()
        {

        }
    }
}