using UnityEngine;
using Zenject;


namespace Sources.RotationLogic
{ 
    public class RotationInputInstaller : MonoInstaller
    {
        [SerializeField] private RotationInput _rotationInput;

        private void OnDisable()
        {
            _rotationInput.Disable();
        }

        public override void InstallBindings()
        {
            _rotationInput.Enable();

            Container.Bind<IRotationInput>().FromInstance(_rotationInput).AsSingle();
        }
    }
}
