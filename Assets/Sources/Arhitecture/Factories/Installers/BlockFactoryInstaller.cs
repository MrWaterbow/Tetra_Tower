using Sources.BlockLogic;
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

        [SerializeField] private GridView _grid;

        private BlockFactory _factory;

        private void OnValidate()
        {
            if(_grid == null)
            {
                _grid = FindObjectOfType<GridView>();
            }
        }

        public override void InstallBindings()
        {
            _factory = new BlockFactory(_startBlock, _blockList, _grid);

            Container.Bind<BlockFactory>().FromInstance(_factory).AsSingle();
        }
    }
}