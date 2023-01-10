using UnityEngine;
using Sources.BuildingLogic;
using DG.Tweening;
using Zenject;
using System.Collections;


namespace Sources.CameraLogic
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform[] _points; // points to move
        [SerializeField] private Transform _cameraTransform; // transform of main camera
        [SerializeField] private Transform _rotationPoint; // point to look at

        [SerializeField] private float _moveTime;
        [SerializeField] private float _rotationTime;

        private BuildingRoot _buildingRoot;
        private int _currentPoint = 0; // represents current point, change controll by this amount where 0 is start point, 1 - right, 2 - opposite and 3 - left]

        [SerializeField] private float _rotationSpeed;

        [Inject]
        private void Construct(BuildingRoot buildingInstaller)
        {
            _buildingRoot = buildingInstaller;
        }
        /// <summary>
        /// Moving camera to needed transform point.
        /// </summary>
        /// <param name="direction"></param>
        public void Move(int direction)
        {
            int pointIndex = GetPointIndex(direction);
            if(direction == -1)
            {
                StartCoroutine(RotateCameraLeft(direction));
            }
            else
            {
                StartCoroutine(RotateCameraRight(direction));
            }
        }

        private IEnumerator RotateCameraLeft(int direction)
        {
            float currentAngle = (int)_rotationPoint.rotation.eulerAngles.y;
            float targetAngle = currentAngle + (-direction * 90);
            while (true)
            {
                float step = _rotationSpeed * Time.deltaTime;
                if(currentAngle + step > targetAngle)
                {
                    step = targetAngle - currentAngle;
                    _rotationPoint.Rotate(Vector3.up, step);
                    break;
                }
                currentAngle += step;
                _rotationPoint.Rotate(Vector3.up, step);
                yield return null;
            }
        }

        private IEnumerator RotateCameraRight(int direction)
        {
            float currentAngle = (int)_rotationPoint.rotation.eulerAngles.y;
            float targetAngle = currentAngle - (direction * 90);
            while (true)
            {
                float step = _rotationSpeed * Time.deltaTime;
                if (currentAngle - step < targetAngle)
                {
                    step = targetAngle - currentAngle;
                    _rotationPoint.Rotate(Vector3.up, step);
                    break;
                }
                currentAngle -= step;
                _rotationPoint.Rotate(Vector3.up, -step);
                yield return null;
            }
        }

        /// <summary>
        /// Getting next/previous value of needed end transform point. 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private int GetPointIndex(int direction)
        {
            // if next point value will be smaller than 0 it should return max point index
            if (_currentPoint + direction < 0)
            {
                _currentPoint = 3;
                _buildingRoot._currentCameraPoint = _currentPoint;
                return _currentPoint;
            }
            // same if index is out of range
            if (_currentPoint + direction > 3)
            {
                _currentPoint = 0;
                _buildingRoot._currentCameraPoint = _currentPoint;
                return _currentPoint;
            }
            // if index value exists, return value
            _currentPoint += direction;
            _buildingRoot._currentCameraPoint = _currentPoint;
            return _currentPoint;
        }
    }
}
