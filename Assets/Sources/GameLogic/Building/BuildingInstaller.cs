using Sources.BlockLogic;
using Sources.Factories;
using Sources.GridLogic;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInstaller : MonoInstaller
    {
        [SerializeField] private int _height;
        [SerializeField] private float _fallTick;

        private List<IBlock> _blocks = new List<IBlock>();

        private IBuildingInput _input;
        private BlockFactory _blockFactory;

        private IBlock _currentBlock;
        private IBlockVisualization _visualization;

        private IGrid _grid;

        private float _tick;

        [Inject]
        private void Construct(IGrid grid, BlockFactory blockFactory, IBlockVisualization visualization, IBuildingInput input)
        {
            _grid = grid;

            _blockFactory = blockFactory;
            _visualization = visualization;
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
            _input.MovingDown += MovingDown;
            _input.MovingRight += MovingRight;
            _input.MovingLeft += MovingLeft;
        }

        private void OnDisable()
        {
            _input.MovingUp -= MovingUp;
            _input.MovingDown -= MovingDown;
            _input.MovingRight -= MovingRight;
            _input.MovingLeft -= MovingLeft;
        }

        private void OnValidate()
        {
            _height = Mathf.Clamp(_height, 0, int.MaxValue);
            _fallTick = Mathf.Clamp(_fallTick, 0, float.MaxValue);

            _currentBlock.Initial
        }

        public override void InstallBindings()
        {
            SpawnNext();
        }

        public void SpawnNext()
        {
            if(_currentBlock != null)
            {
                _currentBlock.Moved -= UpdateVisualization;
                _currentBlock.Placed -= _visualization.Hide;
                _currentBlock.Placed -= AddBlockToList;
                _currentBlock.Placed -= SpawnNext;
            }

            _currentBlock = _blockFactory.Create(_currentBlock == null ? BlockType.Start : BlockType.Random, _height);

            _visualization.Show(_currentBlock.MeshFilter.mesh, _currentBlock.MeshRenderer.sharedMaterial.color);
            UpdateVisualization(_currentBlock.Position);

            _currentBlock.Moved += UpdateVisualization;
            _currentBlock.Placed += _visualization.Hide;
            _currentBlock.Placed += AddBlockToList;
            _currentBlock.Placed += SpawnNext;
        }

        private void UpdateVisualization(Vector3 blockPosition)
        {
            Vector3 visualizationPosition = _currentBlock.Position;

            visualizationPosition.y = 0;

            _visualization.SetPosition(_grid.GetWorldPosition(visualizationPosition) +_currentBlock.OffsetTransform.localPosition);
        }

        private void AddBlockToList()
        {
            _blocks.Add(_currentBlock);
        }

        private void MovingUp()
        {
            _currentBlock.Move(Vector3.forward);
        }

        private void MovingDown()
        {
            _currentBlock.Move(Vector3.forward * -1);
        }

        private void MovingRight()
        {
            _currentBlock.Move(Vector3.right);
        }

        private void MovingLeft()
        {
            _currentBlock.Move(Vector3.left);
        }
    }
}