using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingRootInstaller : MonoInstaller
    {
        [SerializeField] private BuildingRoot _buildingRoot;

        public override void InstallBindings()
        {
            Container.Bind<BuildingRoot>().FromInstance(_buildingRoot).AsSingle();
        }
    }
}
