using Client.BootstrapperLogic;
using DG.Tweening;
using UnityEngine;

namespace Client.CameraLogic
{
    internal sealed class CameraRotation : Bootstrapper
    {
        [SerializeField] private float _mouseDeltaLimit;
        [SerializeField] private float _rotateDuration;

        [Space]

        [SerializeField] private Transform _camera;

        public override void Boot()
        {
            
        }

        private void Update()
        {
            TryRotate();
        }

        private void TryRotate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                float mouseDelta = Input.GetAxis("Mouse X");
                int rotateAngle = mouseDelta > 0 ? 90 : -90;

                if(mouseDelta >= _mouseDeltaLimit)
                {
                    _camera.DORotate((_camera.eulerAngles.y + rotateAngle) * Vector3.up, _rotateDuration);
                }
            }
        }



    }
}