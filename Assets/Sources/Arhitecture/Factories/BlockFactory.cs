using Sources.BlockLogic;
using Sources.GridLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public enum BlockType
    {
        Start,
        Random
    }

    public class BlockFactory : IFactory<BlockType, int, IBlock>
    {
        private readonly BlockView _startTile;
        private readonly BlockView[] _blocksList;

        private readonly IGrid _grid;

        public BlockFactory(BlockView startTile, BlockView[] blocksList, IGrid grid)
        {
            _startTile = startTile;
            _blocksList = blocksList;

            _grid = grid;
        }

        public IBlock Create(BlockType type, int height)
        {
            return Object.Instantiate(GetBlock(type), _grid.GetWorldPosition(Vector3.up * height), Quaternion.identity);
        }

        private BlockView GetBlock(BlockType type)
        {
            return type switch
            {
                BlockType.Start => _startTile,
                BlockType.Random => _blocksList[Random.Range(0, _blocksList.Length)],
                _ => throw new System.Exception("Block type not found!")
            };
        }
    }
}