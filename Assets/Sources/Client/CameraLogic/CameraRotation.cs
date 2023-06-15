using Client.BootstrapperLogic;
using DG.Tweening;
using UnityEngine;

namespace Client.CameraLogic
{
    internal sealed class CameraRotation : MonoBehaviour
    {
        [SerializeField] private float _mouseDeltaLimit;
        [SerializeField] private float _rotateDuration;

        [Space]

        [SerializeField] private Transform _camera;

        private void Update()
        {
            TryRotate();
        }

        private void TryRotate()
        {
            if (Input.GetMouseButton(0))
            {
                RotateCamera();
            }
        }

        private void RotateCamera()
        {
            float mouseDelta = Input.GetAxis("Mouse X");

            if (Mathf.Abs(mouseDelta) >= _mouseDeltaLimit)
            {
                Debug.Log(mouseDelta);

                int rotateAngle = mouseDelta > 0 ? 90 : -90;

                _camera.DORotate((_camera.eulerAngles.y + rotateAngle) * Vector3.up, _rotateDuration);
            }
        }
    }
}