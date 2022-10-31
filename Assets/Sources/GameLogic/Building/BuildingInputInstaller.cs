using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInputInstaller : MonoInstaller
    {
        [SerializeField] private BuildingInput _buildingInput;

        private void OnDisable()
        {
            _buildingInput.Disable();
        }

        public override void InstallBindings()
        {
            _buildingInput.Enable();

            Container.Bind<IBuildingInput>().FromInstance(_buildingInput).AsSingle();
        }
    }
}