using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInputInstaller : MonoInstaller
    {
        [SerializeField] private BuildingInput _buildingInput;

        public override void InstallBindings()
        {
            Container.Bind<IBuildingInput>().FromInstance(_buildingInput).AsSingle();
        }
    }
}