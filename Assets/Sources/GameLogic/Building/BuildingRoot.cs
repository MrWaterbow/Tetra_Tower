using System.Collections.Generic;
using System.Linq;
using System;

using Sources.CompositeRootLogic;
using Sources.RotationLogic;
using Sources.BlockLogic;
using Sources.Factories;
using Sources.GridLogic;

using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingRoot : CompositeRoot
    {
        public event Action SpawnBlock;

        [SerializeField] private int _height;
        [SerializeField] private float _fallTick;

        [Space]

        [SerializeField] private float _moveSmooth;

        private List<Vector3Int> _heightMap = new List<Vector3Int>();
        private List<IBlock> _spawnedBlocks = new List<IBlock>();

        private BlockFactory _blockFactory;

        private IBlockVisualization _visualization;
        private IBuildingInput _input;
        private IBlock _currentBlock;
        private IGrid _grid;

        private float _tick;

        [Inject]
        private void Construct(IGrid grid, BlockFactory blockFactory, IBlockVisualization visualization, IBuildingInput input)
        {
            _blockFactory = blockFactory;

            _visualization = visualization;
            _input = input;
            _grid = grid;
        }

        public IGrid Grid => _grid;

        public float MoveSmooth => _moveSmooth;

        private void Update()
        {
            if (_currentBlock == null) return;

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

            _input.MovingGround += MoveToPlatform;
        }

        private void OnDisable()
        {
            _input.MovingUp -= MovingUp;
            _input.MovingDown -= MovingDown;
            _input.MovingRight -= MovingRight;
            _input.MovingLeft -= MovingLeft;

            _input.MovingGround -= MoveToPlatform;
        }

        private void OnValidate()
        {
            _height = Mathf.Clamp(_height, 0, int.MaxValue);
            _fallTick = Mathf.Clamp(_fallTick, 0, float.MaxValue);
            _moveSmooth = Mathf.Clamp(_moveSmooth, 0, float.MaxValue);
        }

        public override void Init()
        {
            SpawnNext();
        }

        /// <summary>
        /// Spawn next block.
        /// </summary>
        public void SpawnNext()
        {
            _currentBlock = _blockFactory.Create(_currentBlock == null ? BlockType.Start : BlockType.Random, _height);

            _visualization.Show(_currentBlock.MeshFilter.mesh, _currentBlock.MeshRenderer.sharedMaterial.color);
            UpdateVisualizationPosition(_currentBlock.Position);

            _currentBlock.StateMachine.StateChanged += UpdateBlockState;
            _currentBlock.Transforming += UpdateVisualizationPosition;
        }

        private void UpdateBlockState(BlockState newState, BlockState oldState)
        {
            if (newState == BlockState.Placed && oldState == BlockState.Placing)
            {
                _visualization.Hide();
                AddBlock(_currentBlock);
                // Checking nodes HERE....
                UpdateHeight();
                AddHeight();
                SpawnNext();
                SpawnBlock?.Invoke();
            }
        }

        /// <summary>
        /// Find the block by position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>concrete block</returns>
        private IBlock FindBlock(Vector3Int position)
        {
            foreach (IBlock block in _spawnedBlocks)
            {
                foreach (Vector3Int size in block.Size)
                {
                    if (size + block.Position == position)
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return max height of the block ( checking everything block*tile ).
        /// </summary>
        /// <param name="block"></param>
        /// <returns>max height</returns>
        private int GetMaxBlockHeight(IBlock block)
        {
            int max = 0;

            foreach (Vector3 size in block.Size)
            {
                Vector3Int height = _heightMap.FirstOrDefault(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z);

                if (height.y > max)
                {
                    max = height.y;
                }
            }

            return max;
        }

        /// <summary>
        /// Get height by position.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="z">Y position</param>
        /// <returns>value of height</returns>
        private int GetHeight(int x, int z)
        {
            return _heightMap.FirstOrDefault(_ => _.x == x && _.z == z).y;
        }

        /// <summary>
        /// Checking the block moving direction.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="direction"></param>
        /// <returns>true - if move is possible | false - if moving is unpossible</returns>
        public bool CheckMovingDirection(IBlock block, Vector3Int direction)
        {
            if (_currentBlock.StateMachine.CurrentState != BlockState.Placing) return false;

            if (BlockOnPlatfrom(block, direction) && CheckBlockTilesForEntering(block, direction)) return true;

            return false;
        }

        /// <summary>
        /// Check the block tiles for entering into the other blocks.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="direction">moving direction</param>
        /// <returns>true - out of the other blocks | false - entered in the other blocks</returns>
        private bool CheckBlockTilesForEntering(IBlock block, Vector3Int direction)
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

        /// <summary>
        /// Checking the block for entering into the platform.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="direction">moving direction</param>
        /// <returns>true - enter | false - out of the platform</returns>
        private bool BlockOnPlatfrom(IBlock block, Vector3Int direction)
        {
            bool result = false;

            foreach (Vector3Int size in block.Size)
            {
                Vector3Int position = block.Position + direction + size;

                if (OnPlatform(position.x, position.z))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Checking the block position for joined.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool CheckingJoin(IBlock block)
        {
            if (block.Position.y == 0)
            {
                return true;
            }

            bool grounded = false;

            foreach (Vector3 size in block.Size)
            {
                if (_heightMap.FirstOrDefault(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z).y == block.Position.y)
                {
                    grounded = true;
                }
            }

            return grounded;
        }

        /// <summary>
        /// Check position for entering into the platform.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true - enter | false - out of the platform.</returns>
        private bool OnPlatform(int x, int z)
        {
            if (x > -1 && x < 3 && z > -1 && z < 3)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Refreshing the heights list.
        /// </summary>
        private void UpdateHeight()
        {
            List<Vector3Int> refreshed = new();

            foreach (IBlock block in _spawnedBlocks)
            {
                foreach (Vector3Int size in block.Size)
                {
                    // Maybe conflict with the rotating system! Warning!

                    int index = refreshed.FindIndex(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z);

                    if (index == -1)
                    {
                        refreshed.Add(size + block.Position + Vector3Int.up);
                    }
                    else if (size.y + block.Position.y + 1 > refreshed[index].y)
                    {
                        refreshed[index] = size + block.Position + Vector3Int.up;
                    }
                }
            }

            _heightMap.Clear();
            refreshed.ForEach(_ => _heightMap.Add(_));
        }

        /// <summary>
        /// Update visualization position.
        /// </summary>
        /// <param name="blockPosition"></param>
        private void UpdateVisualizationPosition(Vector3 blockPosition)
        {
            Vector3 visualizationPosition = blockPosition;

            visualizationPosition.y = GetMaxBlockHeight(_currentBlock);

            _visualization.SetPosition(_grid.GetWorldPosition(visualizationPosition + _currentBlock.VisualizationOffset));
        }

        /// <summary>
        /// Destroying the block.
        /// </summary>
        /// <param name="block"></param>
        private void DestroyBlock(IBlock block)
        {
            _spawnedBlocks.Remove(block);
            block.Destroy();
        }

        /// <summary>
        /// Instantly move block to platform.
        /// </summary>
        private void MoveToPlatform()
        {
            _currentBlock.Move(Vector3Int.down * (_currentBlock.Position.y - GetMaxBlockHeight(_currentBlock)));
        }

        /// <summary>
        /// Invoke the rigidbody component.
        /// </summary>
        /// <param name="block"></param>
        private void InvokeRigidbody(IBlock block)
        {
            block.InvokeRigidbody();
        }

        /// <summary>
        /// Add block to list.
        /// </summary>
        private void AddBlock(IBlock block)
        {
            _spawnedBlocks.Add(block);
        }

        /// <summary>
        /// Increase spawn height.
        /// </summary>
        private void AddHeight()
        {
            _height++;
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
    }
}