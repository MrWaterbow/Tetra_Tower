using DG.Tweening;
using Sources.BuildingLogic;
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

        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _root;

        private BuildingInstaller _buildingInstaller;

        [Inject]
        private void Construct(BuildingInstaller buildingInstaller)
        {
            _buildingInstaller = buildingInstaller;
        }

        private void Awake()
        {
            _camera.position = _root.position + _offset;
        }

        private void OnEnable()
        {
            _buildingInstaller.NextBlock += MoveCamera;
        }

        private void OnDisable()
        {
            _buildingInstaller.NextBlock -= MoveCamera;
        }

        private void MoveCamera()
        {
            _offset.y += _moveOffset;

            _camera.DOMove(_root.position + _offset, _moveOffset);
        }
    }
}