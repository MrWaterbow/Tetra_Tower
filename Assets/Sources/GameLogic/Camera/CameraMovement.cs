using DG.Tweening;
using Sources.BuildingLogic;
using System;
using UnityEngine;
using Zenject;

namespace Sources.CameraLogic
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        [Space]

        [SerializeField] private float _moveOffset;
        [SerializeField] private float _moveTime;
        [SerializeField] private float _minDelta;

        [Space]

        [SerializeField] private CameraRotation _cameraRotation;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _rotationPoint;
        [SerializeField] private Transform _root;
        [SerializeField] private Camera _camera;

        private BuildingRoot _buildingRoot;

        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        private int _maxHeight;

        [Inject]
        private void Construct(BuildingRoot buildingInstaller)
        {
            _buildingRoot = buildingInstaller;
        }

        private void OnEnable()
        {
            _buildingRoot.SpawnBlock += MoveCamera;
        }

        private void OnDisable()
        {
            _buildingRoot.SpawnBlock -= MoveCamera;
        }

        private void OnValidate()
        {
            _cameraTransform.position = _root.position + _offset;
        }

        private void MoveCamera()
        {
            _maxHeight = _buildingRoot.GetHeighestFromMap();

            _rotationPoint.DOMoveY(_root.position.y + (_maxHeight * _moveOffset), _moveTime);
           // _rotationPoint.position = new Vector3(_rotationPoint.position.x, _rotationPoint.position.y + _moveOffset, _rotationPoint.position.z);
           //_cameraTransform.DOMoveY(_root.position.y + _offset.y + (_maxHeight * _moveOffset), _moveTime);
        }

        private void Update()
        {
#if UNITY_EDITOR
            MouseRotation();
#endif
#if UNITY_EDITOR == false
            MobileRotation();
#endif
        }

        private void MouseRotation()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startTouchPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _endTouchPosition = Input.mousePosition;
                if (Math.Abs(_endTouchPosition.x - _startTouchPosition.x) >= _minDelta)
                {
                    if (_endTouchPosition.x < _startTouchPosition.x)
                    {
                        SwipeLeft();
                    }
                    else if (_endTouchPosition.x > _startTouchPosition.x)
                    {
                        SwipeRight();
                    }
                }
            }
        }

        private void MobileRotation()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _startTouchPosition = Input.GetTouch(0).position; //getting start position of mouse
            }
            if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
            {
                _endTouchPosition = Input.GetTouch(0).position;  //getting end position of mouse
                                                                 //if swipe delta is bigger than minimal value, then deciding right or left swipe
                if (Math.Abs(_endTouchPosition.x - _startTouchPosition.x) >= _minDelta)
                {
                    if (_endTouchPosition.x < _startTouchPosition.x)
                    {
                        SwipeLeft();
                    }
                    else if (_endTouchPosition.x > _startTouchPosition.x)
                    {
                        SwipeRight();
                    }
                }
            }
        }

        private void SwipeRight()
        {
            _cameraRotation.Move(-1);
        }

        private void SwipeLeft()
        {
            _cameraRotation.Move(1);
        }
    }
}