﻿using DG.Tweening;
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

        [Space]

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _root;

        private BuildingInstaller _buildingInstaller;

        [Space]

        [SerializeField] private Camera _camera;
        [SerializeField] Transform _rotationPoint;

        [Space]
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;
        private bool _isMobile;
        [SerializeField] private float _minDelta;
        [SerializeField] private CameraRotation _cameraRotation;

        [Inject]
        private void Construct(BuildingInstaller buildingInstaller)
        {
            _buildingInstaller = buildingInstaller;
        }

        private void Awake()
        {
            _cameraTransform.position = _root.position + _offset;
            _isMobile = Application.isMobilePlatform; //checking platform of app
        }

        private void OnEnable()
        {
            _buildingInstaller.NextBlock += MoveCamera;
        }

        private void OnDisable()
        {
            _buildingInstaller.NextBlock -= MoveCamera;
        }

        private void OnValidate()
        {
            _cameraTransform.position = _root.position + _offset;
        }

        private void MoveCamera()
        {
            _offset.y += _moveOffset;
            _rotationPoint.position = new Vector3(_rotationPoint.position.x, _rotationPoint.position.y + 0.6f, _rotationPoint.position.z);
            _cameraTransform.DOMoveY(_root.position.y + _offset.y, _moveOffset);
        }
       
        private void Update()
        {
            //mobile swipe controll
            if (_isMobile)
            {
                if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _startTouchPosition = Input.GetTouch(0).position; //getting start position of mouse
                }
                if(Input.touchCount>0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
                {
                    _endTouchPosition = Input.GetTouch(0).position;  //getting end position of mouse
                    //if swipe delta is bigger than minimal value, then deciding right or left swipe
                    if (Math.Abs(_endTouchPosition.x - _startTouchPosition.x) >= _minDelta)
                    {
                        if(_endTouchPosition.x < _startTouchPosition.x)
                        {
                            SwipeLeft();
                        }
                        else if(_endTouchPosition.x > _startTouchPosition.x )
                        {
                            SwipeRight();
                        }
                    }
                }
            }
            //editor and others (same as mobile)
            else
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