using Sources.BlockLogic;
using Sources.Factories;
using Sources.GridLogic;
using Sources.RotationLogic;
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

        [Space]

        [SerializeField] private float _moveSmooth;

        private List<Vector3Int> _fullPositions = new List<Vector3Int>();
        private List<IBlock> _blocks = new List<IBlock>();

        private IBuildingInput _input;
        private BlockFactory _blockFactory;

        private IBlock _currentBlock;
        private IBlockVisualization _visualization;

        private IGrid _grid;

        private RotationInstaller _rotation;

        private float _tick;

        [Inject]
        private void Construct(IGrid grid, BlockFactory blockFactory, IBlockVisualization visualization, IBuildingInput input, RotationInstaller rotation)
        {
            _grid = grid;

            _blockFactory = blockFactory;
            _visualization = visualization;
            _input = input;

            _rotation = rotation;
        }

        public float MoveSmooth => _moveSmooth;

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
            _moveSmooth = Mathf.Clamp(_moveSmooth, 0, float.MaxValue);
        }

        public override void InstallBindings()
        {
            Container.Bind<BuildingInstaller>().FromInstance(this).AsSingle();

            SpawnNext();
        }

        public void SpawnNext()
        {
            if (_currentBlock != null)
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
            _currentBlock.Placed += CheckBlockStability;
            _currentBlock.Placed += AddBlock;
            _currentBlock.Placed += ActivePhysics;
            _currentBlock.Placed += AddHeight;
            _currentBlock.Placed += SpawnNext;
            _currentBlock.Placed += NextBlock;

            _rotation.CurrentBlock = _currentBlock;
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

            return grounded;
        }

        public bool CanMove(IBlock block, Vector3Int direction)
        {
            if (block.Position.y == 0) return false;

            //print(CheckCollision(block, direction));

            if (StayOnGround(block, direction) && CheckCollision(block, direction)) return true;

            //if (CheckCollision(block, direction)) return false;

            return false;
        }

        private void UpdateVisualization(Vector3 blockPosition)
        {
            Vector3 visualizationPosition = blockPosition;

            visualizationPosition.y = GetMaxHeight(_currentBlock);

            _visualization.SetPosition(_grid.GetWorldPosition(visualizationPosition + _currentBlock.VisualizationOffset));
        }

        private void CheckBlockStability()
        {
            float joinCount = 0;

            foreach (Vector3Int size in _currentBlock.Size)
            {
                Vector3Int position = size + _currentBlock.Position;

                if (FindJoin(size + _currentBlock.Position))
                {
                    joinCount++;

                    continue;
                }
            }

            print(joinCount);
        }

        private bool FindJoin(Vector3Int position)
        {
            if (position.y == 0 && OnPlatform(position) || FindBlock(position - Vector3Int.up) != null) return true;

            return false;
        }

        private IBlock FindBlock(Vector3Int position)
        {
            foreach (IBlock block in _blocks)
            {
                foreach (Vector3Int size in block.Size)
                {
                    if(size + block.Position == position)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        private void DeleteBlock(IBlock block)
        {
            foreach (Vector3Int size in block.Size)
            {
                RefreshHeight(size + block.Position);
            }

            _blocks.Remove(block);
            block.Destroy();
        }

        private void RefreshHeight(Vector3Int position)
        {
            if(GetHeight(position.x, position.z) == position.y)
            {
                int index = _fullPositions.FindIndex(_ => _.x == position.x && _.z == position.z);

                _fullPositions[index] = position - Vector3Int.up;
            }
        }

        private bool CheckCollision(IBlock block, Vector3Int direction)
        {
            bool result = true;

            foreach (Vector3Int size in block.Size)
            {
                Vector3Int position = block.Position + direction + size;
                int height = GetHeight(position.x, position.z);

                if (position.y <= (height - 1))
                {
                    result = false;
                }
            }

            return result;
        }

        private bool StayOnGround(IBlock block, Vector3Int direction)
        {
            bool result = false;

            foreach (Vector3Int size in block.Size)
            {
                Vector3Int position = block.Position + direction + size;

                if (OnPlatform(position))
                {
                    result = true;
                }
            }

            return result;
        }


        private bool OnPlatform(Vector3Int position)
        {
            if(position.x > -1 && position.x < 3 && position.z > -1 && position.z < 3)
            {
                return true;
            }

            return false;
        }

        private void AddBlock()
        {
            _blocks.Add(_currentBlock);
        }

        private void ActivePhysics()
        {
            _currentBlock.ActivePhysics();
        }

        private void AddHeight()
        {
            _height++;
        }

        private void UpdateFullPositions()
        {
            foreach (Vector3Int size in _currentBlock.Size)
            {
                HeightByPosition(size + _currentBlock.Position + Vector3Int.up);
            }
        }

        private void HeightByPosition(Vector3Int size)
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

        private int GetHeight(int x, int z)
        {
            return _fullPositions.FirstOrDefault(_ => _.x == x && _.z == z).y;
        }

        private bool FindHeight(int x, int z)
        {
            return _fullPositions.Any(_ => _.x == x && _.z == z);
        }

        private int GetMaxHeight(IBlock block)
        {
            int max = 0;

            foreach (Vector3 size in block.Size)
            {
                Vector3Int height = _fullPositions.FirstOrDefault(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z);

                if (height.y > max)
                {
                    max = height.y;
                }
            }
            return max;
        }

        private void MovingUp()
        {
            _currentBlock.Move(Vector3Int.forward);
        }

        private void MovingDown()
        {
            _currentBlock.Move(Vector3Int.forward * -1);
        }

        private void MovingRight()
        {
            _currentBlock.Move(Vector3Int.right);
        }

        private void MovingLeft()
        {
            _currentBlock.Move(Vector3Int.left);
        }

        private void MovingGround()
        {
            _currentBlock.Move(Vector3Int.down * (_currentBlock.Position.y - GetMaxHeight(_currentBlock)));
        }
    }
}