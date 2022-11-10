using Sources.BlockLogic;
using Sources.BuildingLogic;
using Sources.GridLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public class BlockFactoryInstaller : MonoInstaller
    {
        [SerializeField] private BlockView _startBlock;
        [SerializeField] private BlockView[] _blockList;

        [Space]

        [SerializeField] private BuildingInstaller _buildingInstaller;

        private BlockFactory _factory;

        private void OnValidate()
        {
            if(_buildingInstaller == null)
            {
                _buildingInstaller = FindObjectOfType<BuildingInstaller>();
            }
        }

        public override void InstallBindings()
        {
            _factory = new BlockFactory(_startBlock, _blockList, _buildingInstaller);

            Container.Bind<BlockFactory>().FromInstance(_factory).AsSingle();
        }
    }
}