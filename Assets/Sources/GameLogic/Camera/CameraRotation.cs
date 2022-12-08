using UnityEngine;
using DG.Tweening;

namespace Sources.CameraLogic
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform; // transform of main camera
        [SerializeField] private Transform[] _points; // points to move
        [SerializeField] private Transform _rotationPoint; // point to look at
        [SerializeField] private float _moveTime;
        [SerializeField] private float _rotationTime;
        private int _currentPoint = 0; // represents current point, change controll by this amount where 0 is start point, 1 - right, 2 - opposite and 3 - left

        // getting next/previous value of needed end transform point 
        int GetPointIndex(int direction)
        {
            // if next point value will be smaller than 0 it should return max point index
            if (_currentPoint + direction < 0)
            {
                _currentPoint = 3;
                return _currentPoint;
            }
            // same if index is out of range
            if (_currentPoint + direction > 3)
            {
                _currentPoint = 0;
                return _currentPoint;
            }
            // if index value exists, return value
            _currentPoint += direction;
            return _currentPoint;
        }

        // moving camera to needed transform point 
        public void Move(int direction)
        {
            int pointIndex = GetPointIndex(direction);
            _cameraTransform.DODynamicLookAt(_rotationPoint.position, _rotationTime);
            _cameraTransform.DOMoveX(_points[pointIndex].position.x, _moveTime);
            _cameraTransform.DOMoveZ(_points[pointIndex].position.z, _moveTime);
        }
    }
}