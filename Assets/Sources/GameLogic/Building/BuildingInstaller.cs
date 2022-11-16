using Sources.BlockLogic;
using Sources.Factories;
using Sources.GridLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInstaller : MonoInstaller
    {
        public event Action NextBlock;

        [SerializeField] private int _height;
        [SerializeField] private float _fallTick;

        private List<Vector3> _fullPositions = new List<Vector3>();

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

        public IGrid Grid => _grid;

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

            _input.MovingGround += MovingGround;
        }

        private void OnDisable()
        {
            _input.MovingUp -= MovingUp;
            _input.MovingDown -= MovingDown;
            _input.MovingRight -= MovingRight;
            _input.MovingLeft -= MovingLeft;

            _input.MovingGround -= MovingGround;
        }

        private void OnValidate()
        {
            _height = Mathf.Clamp(_height, 0, int.MaxValue);
            _fallTick = Mathf.Clamp(_fallTick, 0, float.MaxValue);
        }

        public override void InstallBindings()
        {
            Container.Bind<BuildingInstaller>().FromInstance(this).AsSingle();

            SpawnNext();
        }

        public void SpawnNext()
        {
            if(_currentBlock != null)
            {
                _currentBlock.Moved -= UpdateVisualization;
                _currentBlock.Placed -= _visualization.Hide;
                _currentBlock.Placed -= UpdateFullPositions;
                _currentBlock.Placed -= AddHeight;
                _currentBlock.Placed -= SpawnNext;
                _currentBlock.Placed -= NextBlock;
            }

            _currentBlock = _blockFactory.Create(_currentBlock == null ? BlockType.Start : BlockType.Random, _height);

            _visualization.Show(_currentBlock.MeshFilter.mesh, _currentBlock.MeshRenderer.sharedMaterial.color);
            UpdateVisualization(_currentBlock.Position);

            _currentBlock.Moved += UpdateVisualization;
            _currentBlock.Placed += _visualization.Hide;
            _currentBlock.Placed += UpdateFullPositions;
            _currentBlock.Placed += AddHeight;
            _currentBlock.Placed += SpawnNext;
            _currentBlock.Placed += NextBlock;
        }

        private void UpdateVisualization(Vector3 blockPosition)
        {
            Vector3 visualizationPosition = blockPosition;

            visualizationPosition.y = GetMaxHeight(_currentBlock);

            _visualization.SetPosition(_grid.GetWorldPosition(visualizationPosition, _currentBlock.HalfSize ? new Vector3(0.55f, 0, 0.65f) : Vector3.zero) + _currentBlock.OffsetTransform.localPosition);
        }

        public bool OnGround(IBlock block)
        {
            if (block.Position.y == 0)
            {
                return true;
            }

            bool grounded = false;

            foreach (Vector3 size in block.Size)
            {
                if (_fullPositions.FirstOrDefault(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z).y == block.Position.y)
                {
                    grounded = true;
                }
            }

            print(grounded);

            return grounded;
        }

        private void AddHeight()
        {
            _height++;
        }

        private void UpdateFullPositions()
        {
            foreach (Vector3 size in _currentBlock.Size)
            {
                HeightByPosition(size + _currentBlock.Position);
            }
        }

        private void HeightByPosition(Vector3 size)
        {
            int index = _fullPositions.FindIndex(_ => _.x == size.x && _.z == size.z);

            if (index == -1)
            {
                _fullPositions.Add(size);
            }
            else
            {
                _fullPositions[index] = size;
            }
        }

        private float GetMaxHeight(IBlock block)
        {
            float max = 0;

            foreach (Vector3 size in block.Size)
            {
                Vector3 height = _fullPositions.FirstOrDefault(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z);

                if (height.y > max)
                {
                    max = height.y;
                }
            }

            return max;
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

        private void MovingGround()
        {
            _currentBlock.Move(Vector3.down * (_currentBlock.Position.y - GetMaxHeight(_currentBlock)));
        }
    }
}