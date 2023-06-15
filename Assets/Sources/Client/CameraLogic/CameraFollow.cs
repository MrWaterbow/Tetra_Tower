using Client.BootstrapperLogic;
using DG.Tweening;
using Server.BrickLogic;
using UnityEngine;
using Zenject;

namespace Client.CameraLogic
{
    internal class CameraFollow : Bootstrapper
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private float _moveFactor;
        [SerializeField] private float _moveDuration;

        private Vector3 _startPosition;

        private BrickMovementWrapper _brickMovementWrapper;

        [Inject]
        private void Constructor(BrickMovementWrapper brickMovementWrapper)
        {
            _brickMovementWrapper = brickMovementWrapper;
        }

        public override void Boot()
        {
            SetCallbacks();
        }

        private void SetCallbacks()
        {
            _brickMovementWrapper.OnControllableBrickFall += UpdateCamera;
        }

        private void UpdateCamera()
        {
            float position = _moveFactor * _brickMovementWrapper.Database.GetHeighestPoint();
            position += _startPosition.y;

            _camera.DOMoveY(position, _moveDuration);
        }

        private void OnValidate()
        {
            _startPosition = _camera.position;
        }
    }
}