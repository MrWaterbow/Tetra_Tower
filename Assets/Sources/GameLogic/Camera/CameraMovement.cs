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

        [Space]

        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _root;

        private BuildingInstaller _buildingInstaller;

        [Space]

        [SerializeField] private Camera _camera;
        [SerializeField] private float _sensetivity;
        [SerializeField] Transform _rotationPoint;

        private Vector3 _previousPos;

        [Inject]
        private void Construct(BuildingInstaller buildingInstaller)
        {
            _buildingInstaller = buildingInstaller;
        }

        private void Awake()
        {
            _cameraTransform.position = _root.position + _offset;
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
            //_camera.transform.position = new Vector3(transform.position.x, _root.position.y+_offset.y, transform.position.z);
            _cameraTransform.DOMove(_root.position + _offset, _moveOffset);
        }
       
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousPos = _camera.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = _previousPos - _camera.ScreenToViewportPoint(Input.mousePosition);
                _camera.transform.RotateAround(_rotationPoint.position, Vector3.up, -direction.x * _sensetivity);
            }
        }
    }
}