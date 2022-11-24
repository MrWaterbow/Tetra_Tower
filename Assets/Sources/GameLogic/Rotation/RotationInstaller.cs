using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sources.BlockLogic;
using Sources.BuildingLogic;
using Sources.RotationLogic;


namespace Sources.RotationLogic 
{
    public class RotationInstaller : MonoInstaller
    {
        public IBlock CurrentBlock;
        private IRotationInput _input;

        [Inject]
        private void Construct(IRotationInput input)
        {
            _input = input;
        }

        private void OnEnable()
        {
            _input.RotateUp += RotateUp;
            _input.RotateDown += RotateDown;
            _input.RotateRight += RotateRight;
            _input.RotateLeft += RotateLeft;
        }

        private void OnDisable()
        {
            _input.RotateUp -= RotateUp;
            _input.RotateDown -= RotateDown;
            _input.RotateRight -= RotateRight;
            _input.RotateLeft -= RotateLeft;
        }
        public override void InstallBindings()
        {
            Container.Bind<RotationInstaller>().FromInstance(this).AsSingle();
        }

        private void RotateUp()
        {
            CurrentBlock.Rotate(Vector3.right, 90);
        }
        private void RotateDown()
        {
            CurrentBlock.Rotate(Vector3.right, -90);
        }
        private void RotateRight()
        {
            CurrentBlock.Rotate(Vector3.up, 90);
        }
        private void RotateLeft()
        {
            CurrentBlock.Rotate(Vector3.up, -90);
        }
    }
}

