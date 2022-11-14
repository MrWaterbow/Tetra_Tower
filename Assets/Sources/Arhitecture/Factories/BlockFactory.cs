﻿using Sources.BlockLogic;
using Sources.BuildingLogic;
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

        private readonly BuildingInstaller _buildingInstaller;

        public BlockFactory(BlockView startTile, BlockView[] blocksList, BuildingInstaller buildingInstaller)
        {
            _startTile = startTile;
            _blocksList = blocksList;

            _buildingInstaller = buildingInstaller;
        }

        public IBlock Create(BlockType type, int height)
        {
            BlockView instance = Object.Instantiate(GetBlock(type), _buildingInstaller.Grid.GetWorldPosition(Vector3.up * height, Vector3.zero), Quaternion.identity);
            
            instance.Initialize(Vector3.up * height, _buildingInstaller);

            return instance;
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