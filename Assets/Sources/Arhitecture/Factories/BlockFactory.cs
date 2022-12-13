using Sources.BlockLogic;
using Sources.BuildingLogic;
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
        private readonly BlockView[] _blocks;
        private readonly Color[] _blockColors;

        private readonly BlockView _startTile;
        private readonly BuildingRoot _buildingRoot;

        private int _renderIndex = 3000;

        public BlockFactory(BlockView startTile, BlockView[] blocks, Color[] colors, BuildingRoot buildingRoot)
        {
            _blocks = blocks;
            _blockColors = colors;

            _startTile = startTile;
            _buildingRoot = buildingRoot;
        }

        public int RenderIndex => _renderIndex;

        public IBlock Create(BlockType type, int height)
        {
            BlockView instance = Object.Instantiate(GetBlock(type), _buildingRoot.Grid.GetWorldPosition(Vector3.up * height), Quaternion.identity);
            
            instance.MeshRenderer.material.color = _blockColors[Random.Range(0, _blockColors.Length)]; 
            //instance.MeshRenderer.material.renderQueue = _renderIndex;
            instance.Initialize(Vector3Int.up * height, _buildingRoot);

            _renderIndex++;

            return instance;
        }

        private BlockView GetBlock(BlockType type)
        {
            return type switch
            {
                BlockType.Start => _startTile,
                BlockType.Random => _blocks[Random.Range(0, _blocks.Length)],
                _ => throw new System.Exception("Block type not found!")
            };
        }
    }
}