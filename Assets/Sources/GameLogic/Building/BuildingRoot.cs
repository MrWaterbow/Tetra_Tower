using Sources.BlockLogic;
using Sources.CompositeRootLogic;
using Sources.Factories;
using Sources.GridLogic;
using Sources.RotationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

//private float GetUnderJoins(IBlock block, IBlock blocking)
//{
//    float count = 0;

//    foreach (Vector3Int size in block.Size)
//    {
//        if (GetJoin(size + block.Position, Vector3Int.down, blocking))
//        {
//            List<Vector3Int> positions = new();
//            float distance = 0f;

//            foreach (Vector3Int size2 in block.Size)
//            {
//                bool underBlock = GetJoin(size2 + block.Position, Vector3Int.down, blocking);

//                if (underBlock == false)
//                {
//                    positions.Add(size2);
//                }
//            }

//            foreach (Vector3Int position in positions)
//            {
//                if (distance == 0f)
//                {
//                    distance = Vector3.Distance(size, position);
//                }
//                else if (distance > Vector3.Distance(size, position))
//                {
//                    distance = Vector3.Distance(size, position);
//                }
//            }

//            if (distance == 0) distance = 1;

//            count += 1 + (distance - 1);
//        }
//        else
//        {
//            List<Vector3Int> positions = new();
//            float distance = 0f;

//            foreach (Vector3Int size1 in block.Size)
//            {
//                bool underBlock = GetJoin(size1 + block.Position, Vector3Int.down, blocking);

//                if (underBlock)
//                {
//                    positions.Add(size1);
//                }
//            }

//            foreach (Vector3Int position in positions)
//            {
//                if (distance == 0f)
//                {
//                    distance = Vector3.Distance(size, position);
//                }
//                else if (distance > Vector3.Distance(size, position))
//                {
//                    distance = Vector3.Distance(size, position);
//                }
//            }

//            if (distance == 0) distance = 1;

//            count -= distance - 1;
//        }
//    }

//    return count;

//    //float count = 0;

//    //foreach (Vector3Int size in block.Size)
//    //{
//    //    if (GetJoin(size + block.Position, Vector3Int.down, blocking))
//    //    {
//    //        count += 1 * GetTileDistance(block, size + block.Position);
//    //    }
//    //}

//    //return count;
//}

namespace Sources.BuildingLogic
{
    public class BuildingRoot : CompositeRoot
    {
        public event Action SpawnBlock;

        [SerializeField] private int _startHeight;
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

        private RotationInstaller _rotationInstaller;

        private float _tick;

        private int _height;

        private Vector3Int[] _currentDirection = new Vector3Int[4];

        private Vector3Int[][] _moveDirections =
        {
            new Vector3Int[] {Vector3Int.forward, Vector3Int.forward * -1, Vector3Int.right, Vector3Int.left},
            new Vector3Int[] {Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.forward *-1},
            new Vector3Int[] {Vector3Int.forward *-1, Vector3Int.forward, Vector3Int.left, Vector3Int.right},
            new Vector3Int[] {Vector3Int.right, Vector3Int.left, Vector3Int.forward * -1, Vector3Int.forward}
        };

        [Inject]
        private void Construct(IGrid grid, BlockFactory blockFactory, IBlockVisualization visualization, IBuildingInput input, RotationInstaller rotationInstaller)
        {
            _blockFactory = blockFactory;

            _visualization = visualization;
            _input = input;
            _grid = grid;

            _rotationInstaller = rotationInstaller;
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
            ComputerControll();
        }

        private void ComputerControll()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MovingUp();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MovingLeft();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MovingDown();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MovingRight();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                MoveToPlatform();
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
            _startHeight = Mathf.Clamp(_startHeight, 0, int.MaxValue);
            _fallTick = Mathf.Clamp(_fallTick, 0, float.MaxValue);
            _moveSmooth = Mathf.Clamp(_moveSmooth, 0, float.MaxValue);
        }

        public override void Init()
        {
            _height = _startHeight;

            SpawnNext();
        }

        /// <summary>
        /// Spawn next block.
        /// </summary>
        public void SpawnNext()
        {
            _currentBlock = _blockFactory.Create(_currentBlock == null ? BlockType.Start : BlockType.Random, _height);

            _visualization.Show(_currentBlock.MeshFilter.mesh, _currentBlock.MeshRenderer.sharedMaterial.color);
            //_visualization.SetRenderQueue(_blockFactory.RenderIndex + 2);
            UpdateVisualizationPosition(_currentBlock.Position);

            _currentBlock.StateMachine.StateChanged += UpdateBlockState;
            _currentBlock.Transforming += UpdateVisualizationPosition;
            _rotationInstaller.CurrentBlock = _currentBlock;
        }

        public int GetHeighestFromMap()
        {
            int max = 0;

            foreach (Vector3Int map in _heightMap)
            {
                if (max < map.y)
                {
                    max = map.y;
                }
            }

            return max;
        }

        private void UpdateBlockState(BlockState newState, BlockState oldState)
        {
            if (newState == BlockState.Placed && oldState == BlockState.Placing)
            {
                _visualization.Hide();
                AddBlock(_currentBlock);
                CheckStability();
                UpdateHeightMap();
                UpdateBlockHeight();

                GetTileDistance(_currentBlock, new Vector3Int(0, 1, 0));

                SpawnNext();
                SpawnBlock?.Invoke();
            }
        }

        /// <summary>
        /// Find the block by position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>concrete block</returns>
        private IBlock FindBlock(Vector3Int position, IBlock blocking = null)
        {
            foreach (IBlock block in _spawnedBlocks)
            {
                if (block == blocking) continue;

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
            if (block.StateMachine.CurrentState != BlockState.Placing) return false;

            if (BlockOnPlatfrom(block, direction) && CheckBlockTilesForEntering(block, direction)) return true;

            return false;
        }

        /// <summary>
        /// Check the block joins
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool CheckPlaced(IBlock block)
        {
            if (block.StateMachine.CurrentState != BlockState.Placing) return false;

            if (block.Position.y == 0) return true;

            if (GetMaxBlockHeight(block) >= block.Position.y) return true;

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

                if (OnPlatform(position.x, position.y, position.z, block))
                {
                    result = true;
                }
            }

            return result;
        }

        //private float GetUnderJoins(IBlock block)
        //{
        //    float count = 0;

        //    foreach (Vector3Int size in block.Size)
        //    {
        //        if (GetJoin(size + block.Position, Vector3Int.down))
        //        {
        //            count++;
        //        }
        //    }

        //    return count;
        //}

        private float GetUnderJoins(IBlock block, IBlock blocking)
        {
            float count = 0;

            foreach (Vector3Int size in block.Size)
            {
                if (GetJoin(size + block.Position, Vector3Int.down, blocking))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Return joins count
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private float GetUnderJoins(IBlock block)
        {
            float count = block.Size.Length;

            foreach (Vector3Int size in block.Size)
            {
                if(GetJoin(size + block.Position, Vector3Int.down))
                {
                    count += GetDistance(block, block.Position + size, false) - 1;
                }
                else
                {
                    count -= GetDistance(block, block.Position + size, true);
                }
            }

            //foreach (Vector3Int size in block.Size)
            //{
            //    if (GetJoin(size + block.Position, Vector3Int.down))
            //    {
            //        List<Vector3Int> positions = new();
            //        float distance = 0f;

            //        foreach (Vector3Int size2 in block.Size)
            //        {
            //            bool underBlock = GetJoin(size2 + block.Position, Vector3Int.down);

            //            if (underBlock == false)
            //            {
            //                positions.Add(size2);
            //            }
            //        }

            //        foreach (Vector3Int position in positions)
            //        {
            //            if (distance == 0f)
            //            {
            //                distance = Vector3.Distance(size, position);
            //            }
            //            else if (distance > Vector3.Distance(size, position))
            //            {
            //                distance = Vector3.Distance(size, position);
            //            }
            //        }

            //        if (distance == 0) distance = 1;

            //        count += 1 + (distance - 1);
            //    }
            //    else
            //    {
            //        List<Vector3Int> positions = new();
            //        float distance = 0f;

            //        foreach (Vector3Int size1 in block.Size)
            //        {
            //            bool underBlock = GetJoin(size1 + block.Position, Vector3Int.down);

            //            if (underBlock)
            //            {
            //                positions.Add(size1);
            //            }
            //        }

            //        foreach (Vector3Int position in positions)
            //        {
            //            if (distance == 0f)
            //            {
            //                distance = Vector3.Distance(size, position);
            //            }
            //            else if (distance > Vector3.Distance(size, position))
            //            {
            //                distance = Vector3.Distance(size, position);
            //            }
            //        }

            //        if (distance == 0) distance = 1;

            //        count -= distance - 1;
            //    }
            //}

            return count;
        }

        private Vector3Int GetCenterOfMass(IBlock block)
        {
            foreach (Vector3Int size in block.Size)
            {
                if(GetJoin(block.Position + size, Vector3Int.down) == false)
                {
                    return size;
                }
            }

            throw new Exception("WTF!!!111");
        }

        private float GetDistance(IBlock block, Vector3Int point, bool haveBlock)
        {
            List<Vector3Int> positions = new();

            foreach (Vector3Int size in block.Size)
            {
                if(GetJoin(size + block.Position, Vector3Int.down) == haveBlock)
                {
                    positions.Add(size + block.Position);
                }
            }

            if (positions.Count == 0) return 1;

            float distance = Vector3.Distance(point, positions[0]);

            foreach (Vector3Int position in positions)
            {
                if(distance > Vector3.Distance(point, position))
                {
                    distance = Vector3.Distance(point, position);
                }
            }

            return distance;
        }

        /// <summary>
        /// Get upper joins count
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private float GetUpperJoins(IBlock block, float underArea)
        {
            float count = 0;

            foreach (Vector3Int size in block.Size)
            {
                if (GetJoin(size + block.Position, Vector3Int.up) && underArea > 0.5f)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Checking blocks stability
        /// </summary>
        private void CheckStability()
        {
            List<IBlock> destroyingBlocks = new();

            foreach (IBlock block in _spawnedBlocks.Reverse<IBlock>())
            {
                float underJoins = GetUnderJoins(block); // Get under joins count
                float oppositeJoins = GetUpperJoins(block, underJoins / block.Size.Length); // Get upper join

                foreach (IBlock upper in GetUpperBlocks(block))
                {
                    if (GetUnderJoins(upper, block) > 0)
                    {
                        oppositeJoins = 99;
                    }
                }

                float joinArea = (underJoins + oppositeJoins) / block.Size.Length;

                if (joinArea == 0.5f)
                {
                    if (block.Instable)
                    {
                        destroyingBlocks.Add(block);

                        foreach (IBlock node in GetUpperBlocks(block))
                        {
                            if (IsInstable(node))
                            {
                                destroyingBlocks.Add(node);
                            }
                        }
                    }
                    else block.InvokeInstable();
                }
                else if (joinArea < 0.5f)
                {
                    destroyingBlocks.Add(block);

                    foreach (IBlock node in GetUpperBlocks(block))
                    {
                        if (IsInstable(node))
                        {
                            destroyingBlocks.Add(node);
                        }
                    }
                }
            }

            destroyingBlocks.ForEach(_ => DestroyBlock(_));

            FixStability();
        }

        /// <summary>
        /// Checking blocks stability after destroying blocks
        /// </summary>
        private void FixStability()
        {
            List<IBlock> destroyingBlocks = new();

            foreach (IBlock block in _spawnedBlocks)
            {
                float underJoins = GetUnderJoins(block);
                float oppositeJoins = GetUpperJoins(block, underJoins / block.Size.Length);

                foreach (IBlock upper in GetUpperBlocks(block))
                {
                    if (GetUnderJoins(upper) > 0)
                    {
                        oppositeJoins = 99;
                    }
                }

                float joinArea = (underJoins + oppositeJoins) / block.Size.Length;

                if (joinArea == 0.5f)
                {
                    block.InvokeInstable();
                }
                else if (joinArea < 0.5)
                {
                    destroyingBlocks.Add(block);
                }
                else
                {
                    block.DeinvokeInstable();
                }
            }

            destroyingBlocks.ForEach(_ => DestroyBlock(_));
        }

        /// <summary>
        /// Checking block stability
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsInstable(IBlock block)
        {
            float joinArea = GetUnderJoins(block) / block.Size.Length;

            if (joinArea == 0.5f)
            {
                if (block.Instable) return true;

                else block.InvokeInstable();
            }
            else if (joinArea < 0.5f)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return join checking by position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool GetJoin(Vector3Int position, Vector3Int offset, IBlock blocking = null)
        {
            if (position.y + offset.y <= -1 && OnPlatform(position.x, position.y, position.z, _currentBlock))//((yOffset ? position.y + offset.y : position.y) <= 0 && OnPlatform(position.x, position.z))
            {
                return true;
            }

            if (FindBlock(position + offset, blocking) != null)
            {
                return true;
            }

            return false;
        }

        //private int GetJoinDistanceMultiply(IBlock block, Vector3Int position)
        //{
        //    List<IBlock> blocks = GetUnderBlocks(block);

        //    foreach (IBlock underBlock in blocks)
        //    {

        //    }
        //}

        /// <summary>
        /// Get all upper blocks
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private List<IBlock> GetUpperBlocks(IBlock start)
        {
            List<IBlock> result = new();

            foreach (Vector3Int size in start.Size)
            {
                IBlock block = FindBlock(size + start.Position + Vector3Int.up);

                if (block != null && result.Any(_ => _ == block) == false)
                {
                    result.Add(block);

                    result.AddRange(GetUpperBlocks(block));
                }
            }

            return result;
        }

        /// <summary>
        /// Get all under blocks
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private List<IBlock> GetUnderBlocks(IBlock start)
        {
            List<IBlock> result = new();

            foreach (Vector3Int size in start.Size)
            {
                IBlock block = FindBlock(size + start.Position + Vector3Int.down);

                if (block != null && result.Any(_ => _ == block) == false)
                {
                    result.Add(block);
                }
            }

            return result;
        }

        private float GetTileDistance(IBlock block, Vector3Int blockTile)
        {
            return 0f;

            //float distance = 0f;

            //List<Vector3Int> positions = new();
            //float distance = 0f;

            //foreach (Vector3Int size in block.Size)
            //{
            //    bool underBlock = GetJoin(size + block.Position, Vector3Int.down);

            //    if (underBlock)
            //    {
            //        positions.Add(size + block.Position + Vector3Int.up);
            //    }
            //}

            //foreach (Vector3Int position in positions)
            //{
            //    if (distance == 0f)
            //    {
            //        distance = Vector3.Distance(position, blockTile);
            //    }
            //    else if (distance > Vector3.Distance(position, blockTile))
            //    {
            //        distance = Vector3.Distance(position, blockTile);
            //    }
            //}

            //return distance;
        }

        //private float GetTileDistance(IBlock block, Vector3Int blockTile)
        //{
        //    List<Vector3Int> positions = new();
        //    float distance = 0f;

        //    foreach (Vector3Int size in block.Size)
        //    {
        //        bool underBlock = GetJoin(size + block.Position, Vector3Int.down);

        //        if (underBlock)
        //        {
        //            positions.Add(size + block.Position + Vector3Int.up);
        //        }
        //    }

        //    foreach (Vector3Int position in positions)
        //    {
        //        if (distance == 0f)
        //        {
        //            distance = Vector3.Distance(position, blockTile);
        //        }
        //        else if (distance > Vector3.Distance(position, blockTile))
        //        {
        //            distance = Vector3.Distance(position, blockTile);
        //        }
        //    }

        //    return distance;
        //}

        private List<IBlock> SortingInstableBlocks(List<IBlock> blocks)
        {
            List<IBlock> result = new();

            foreach (IBlock block in blocks)
            {
                if (GetUnderJoins(block) <= 0.5f)
                {
                    result.Add(block);
                }
            }

            return result;
        }

        /// <summary>
        /// Check position for entering into the platform.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true - enter | false - out of the platform.</returns>
        private bool OnPlatform(int x, int y, int z, IBlock block)
        {
            //if(position.y > 0 && GetUnderBlocks(block).Count > 0)
            //{

            //}

            //if(y > 0 && GetUnderBlocks(block).Count > 0)
            //{
            //    return true;
            //}

            if(_heightMap.Any(_ => _.x == x && _.z == z))
            {
                return true;
            }

            if (x > -1 && x < 3 && z > -1 && z < 3)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Refreshing the heights map.
        /// </summary>
        private void UpdateHeightMap()
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

        private bool HaveExtensionBlock(int x, int y, int z, IBlock block)
        {
            foreach (Vector3Int size in block.Size)
            {
                if(_heightMap.Any(_ => _.x == size.x + block.Position.x && _.z == size.z + block.Position.z))
                {
                    return true;
                }
            }

            return false;
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
            if (_spawnedBlocks.Any(_ => _ == block) == false) return;

            _spawnedBlocks.Remove(block);

            block.Destroy();
            block.SetCenterOfMass(GetCenterOfMass(block) * 3);
        }

        /// <summary>
        /// Instantly move block to platform.
        /// </summary>
        private void MoveToPlatform()
        {
            _currentBlock.Move(Vector3Int.down * (_currentBlock.Position.y - GetMaxBlockHeight(_currentBlock)));
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
        private void UpdateBlockHeight()
        {
            _height = GetHeighestFromMap() + _startHeight;
        }


        public int _currentCameraPoint;
        private void SetMoveDirection()
        {
            _currentDirection = _moveDirections[_currentCameraPoint];
        }

        private void MovingUp()
        {
            SetMoveDirection();
            _currentBlock.Move(_currentDirection[0]);
        }

        private void MovingDown()
        {
            SetMoveDirection();
            _currentBlock.Move(_currentDirection[1]);
        }

        private void MovingRight()
        {
            SetMoveDirection();
            _currentBlock.Move(_currentDirection[2]);
        }

        private void MovingLeft()
        {
            SetMoveDirection();
            _currentBlock.Move(_currentDirection[3]);
        }
    }
}