using Sources.BlockLogic;
using Sources.BuildingLogic;
using Sources.GridLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public class BlockFactoryInstaller : MonoInstaller
    {
        [SerializeField] private BlockView[] _blocks;
        [SerializeField] private Color[] _blockColors;

        [Space]

        [SerializeField] private BlockView _startBlock;

        [Space]

        [SerializeField] private BuildingRoot _buildingRoot;

        private BlockFactory _factory;

        private void OnValidate()
        {
            if(_buildingRoot == null)
            {
                _buildingRoot = FindObjectOfType<BuildingRoot>();
            }
        }

        public override void InstallBindings()
        {
            _factory = new BlockFactory(_startBlock, _blocks, _blockColors, _buildingRoot);

            Container.Bind<BlockFactory>().FromInstance(_factory).AsSingle();
        }
    }
}