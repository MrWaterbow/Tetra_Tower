using Sources.BlockLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public class BlockVisualizationInstaller : MonoInstaller
    {
        [SerializeField] private BlockVisualization _prefab;

        private BlockVisualizationFactory _factory;

        public override void InstallBindings()
        {
            _factory = new BlockVisualizationFactory(_prefab);

            IBlockVisualization instance = _factory.Create();

            instance.Hide();

            Container.Bind<IBlockVisualization>().FromInstance(instance).AsSingle();
        }
    }
}