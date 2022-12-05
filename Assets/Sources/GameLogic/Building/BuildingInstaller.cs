using Sources.BlockLogic;
using Sources.Factories;
using Sources.GridLogic;
using Sources.RotationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
                _currentBlock.Moved -= UpdateVisualizationPosition;
                _currentBlock.Placed -= _visualization.Hide;
                //_currentBlock.Placed += UpdateFullPositions;
                _currentBlock.Placed -= AddBlock;
                _currentBlock.Placed -= UpdateBlocksState;
                _currentBlock.Placed -= RefreshHeight;
                // _currentBlock.Placed += ActivePhysics;
                _currentBlock.Placed -= AddHeight;
                _currentBlock.Placed -= SpawnNext;
                _currentBlock.Placed -= NextBlock;
            }

            _currentBlock = _blockFactory.Create(_currentBlock == null ? BlockType.Start : BlockType.Random, _height);

            _visualization.Show(_currentBlock.MeshFilter.mesh, _currentBlock.MeshRenderer.sharedMaterial.color);
            UpdateVisualizationPosition(_currentBlock.Position);

            _currentBlock.Moved += UpdateVisualizationPosition;
            _currentBlock.Placed += _visualization.Hide;
            //_currentBlock.Placed += UpdateFullPositions;
            _currentBlock.Placed += AddBlock;
            _currentBlock.Placed += UpdateBlocksState;
            _currentBlock.Placed += RefreshHeight;
            // _currentBlock.Placed += ActivePhysics;
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

        private void UpdateBlocksState()
        {
            //List<IBlock> destroyingBlocks = new();
            //List<IBlock> nodes = new();

            //foreach (IBlock block in _blocks)
            //{
            //    float joinArea = GetJoinArea(block);

            //    if (joinArea == 0.5f)
            //    {
            //        if (block.Instable) destroyingBlocks.Add(block);
            //        else if(block.Position.y == 0) block.MakeInstable();
            //    }
            //    else if (joinArea < 0.5f)
            //    {
            //        destroyingBlocks.Add(block);
            //    }
            //}

            //foreach (IBlock block2 in _blocks)
            //{
            //    GetNode(block2).ForEach(_ => nodes.Add(_));
            //}

            //destroyingBlocks.ForEach(_ => DestroyBlock(_));

            //foreach (IBlock block1 in destroyingBlocks)
            //{
            //    GetNode(block1).ForEach(_ => nodes.Add(_));
            //}

            //GetInstableBlock(nodes).ForEach(_ => DestroyBlock(_));
        }

        /// <summary>
        /// Get upper connected blocks
        /// </summary>
        /// <param name="block"></param>
        /// <returns>List of the connected blocks</returns>
        private List<IBlock> GetNode(IBlock block)
        {
            List<IBlock> result = new();

            foreach (Vector3Int size in block.Size)
            {
                IBlock node = FindBlock(block.Position + size + Vector3Int.up);

                if (node != null && result.Any(_ => _ == node) == false && GetJoinArea(node) != 1)
                {
                    result.Add(node);

                    GetNode(node).ForEach(_ => result.Add(_));
                }

            }

            return result;
        }

        /// <summary>
        /// Checking the block stability. Make the block instable.
        /// </summary>
        /// <param name="block"></param>
        /// <returns>true - if block falling | false - if block instable/normal</returns>
        private bool CheckBlockStability(IBlock block)
        {
            float joinArea = GetJoinArea(block);

            if (joinArea == 0.5f)
            {
                if (block.Instable) return true;
                else if (block.Position.y == 0) block.MakeInstable();
            }
            else if (joinArea < 0.5f)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return block join area. The area of contact of the block with the surface.
        /// </summary>
        /// <param name="block">concrete block</param>
        /// <returns>Join area is a value from 0 to 1.</returns>
        private float GetJoinArea(IBlock block)
        {
            float joinCount = 0;

            foreach (Vector3Int size in block.Size)
            {
                if (FindDownJoin(size + block.Position))
                {
                    joinCount++;

                    continue;
                }
            }

            return joinCount / block.Size.Length;
        }

        // MAYBE OLD
        private bool FindDownJoin(Vector3Int position)
        {
            if (position.y == 0 && OnPlatform(position.x, position.z) || FindBlock(position - Vector3Int.up) != null) return true;

            return false;
        }

        /// <summary>
        /// Find the block by position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>concrete block</returns>
        private IBlock FindBlock(Vector3Int position)
        {
            foreach (IBlock block in _blocks)
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
        /// Refreshing the heights list
        /// </summary>
        private void RefreshHeight()
        {
            List<Vector3Int> refreshed = new();

            foreach (IBlock block in _blocks)
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

            _fullPositions.Clear();
            refreshed.ForEach(_ => _fullPositions.Add(_));
        }


        /// <summary>
        /// Update visualization position
        /// </summary>
        /// <param name="blockPosition"></param>
        private void UpdateVisualizationPosition(Vector3 blockPosition)
        {
            Vector3 visualizationPosition = blockPosition;

            visualizationPosition.y = GetMaxBlockHeight(_currentBlock);

            _visualization.SetPosition(_grid.GetWorldPosition(visualizationPosition + _currentBlock.VisualizationOffset));
        }

        /// <summary>
        /// Checking the block moving direction.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="direction"></param>
        /// <returns>true - if move is possible | false - if moving is unpossible</returns>
        public bool CheckMovingDirection(IBlock block, Vector3Int direction)
        {
            if (block.Position.y == 0) return false;

            if (BlockOnPlatfrom(block, direction) && CheckBlockTilesForEntering(block, direction)) return true;

            return false;
        }

        /// <summary>
        /// Check the block tiles for entering into the other blocks
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
        /// Checking the block for entering into the platform
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
        /// Return max height of the block ( checking everything block*tile )
        /// </summary>
        /// <param name="block"></param>
        /// <returns>max height</returns>
        private int GetMaxBlockHeight(IBlock block)
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

        /// <summary>
        /// Check position for entering into the platform
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true - enter | false - out of the platform</returns>
        private bool OnPlatform(int x, int z)
        {
            if (x > -1 && x < 3 && z > -1 && z < 3)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Destroying the block
        /// </summary>
        /// <param name="block"></param>
        private void DestroyBlock(IBlock block)
        {
            _blocks.Remove(block);
            block.Destroy();
        }

        /// <summary>
        /// Get height by position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="z">Y position</param>
        /// <returns>value of height</returns>
        private int GetHeight(int x, int z)
        {
            return _fullPositions.FirstOrDefault(_ => _.x == x && _.z == z).y;
        }

        /// <summary>
        /// Add block to list
        /// </summary>
        private void AddBlock()
        {
            _blocks.Add(_currentBlock);
        }

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

        private void MovingGround()
        {
            _currentBlock.Move(Vector3Int.down * (_currentBlock.Position.y - GetMaxBlockHeight(_currentBlock)));
        }
    }
}