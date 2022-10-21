using Sources.BlockLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public enum BlockType
    {
        Start,
        Random
    }

    public class BlockFactoryInstaller : IFactory<BlockType, BlockView>
    {
        private readonly BlockView _startTile;
        private readonly BlockView[] _blocksList;

        public BlockFactoryInstaller(BlockView startTile, BlockView[] blocksList)
        {
            _startTile = startTile;
            _blocksList = blocksList;
        }

        public BlockView Create(BlockType type)
        {
            return Object.Instantiate(GetBlock(type)); // TODO
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