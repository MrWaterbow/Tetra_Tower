using Sources.BlockLogic;
using UnityEngine;
using Zenject;

namespace Sources.Factories
{
    public enum VisualizationType
    {
        Full,
        Transparency
    }

    public class BlockVisualizationFactory : IFactory<IBlockVisualization>
    {
        private readonly BlockVisualization _prefab;

        public BlockVisualizationFactory(BlockVisualization prefab)
        {
            _prefab = prefab;
        }

        public IBlockVisualization Create()
        {
            return Object.Instantiate(_prefab);
        }
    }
}