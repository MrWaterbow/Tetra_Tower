using DG.Tweening;
using Sources.BuildingLogic;
using System;
using System.Linq;
using UnityEngine;

namespace Sources.BlockLogic
{
    public class BlockView : MonoBehaviour, IBlock
    {
        public event Action<Vector3> Moved;
        public event Action Placed;

        [SerializeField] private Vector3Int[] _size;
        [SerializeField] private Vector3 _visualizationOffset;

        [Space]

        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        private BuildingInstaller _buildingInstaller;

        private Vector3Int _position;

        public Vector3Int[] Size => _size;
        public Vector3 VisualizationOffset => _visualizationOffset;

        public Vector3Int Position => _position;

        public Transform Transform => _transform;
        public Transform ModelTransform => _modelTransform;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshFilter MeshFilter => _meshFilter;

        private void OnValidate()
        {
            if (_transform == null)
            {
                _transform = transform;
            }

            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (Vector3 size in _size)
            {
                Gizmos.color = new Color(0, 0, 0, 0.4f);

                Gizmos.DrawCube(_transform.position + size, Vector3.one * 1.1f);
            }
        }

        public void Initialize(Vector3Int position, BuildingInstaller buildingInstaller)
        {
            _position = position;

            _buildingInstaller = buildingInstaller;
        }

        public void Fall()
        {
            if (_position.y == 0) return;

            _position.y--;

            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position), _buildingInstaller.MoveSmooth);

            CheckGrounded();

            Moved?.Invoke(Position);
        }

        public void Move(Vector3Int direction)
        {
            if (CanMove(direction) == false) return;

            _position += direction;

            OnMoving();
        }

        private void OnMoving()
        {
            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position), _buildingInstaller.MoveSmooth);

            CheckGrounded();

            Moved?.Invoke(Position);
        }

        private bool CanMove(Vector3Int direction)
        {
            return _buildingInstaller.CanMove(this, direction);
        }

        private void CheckGrounded()
        {
            if (_buildingInstaller.OnGround(this))
            {
                Placed?.Invoke();
            }
        }

        public void Rotate(Vector3 direction, int degree)
        {
            _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
            //Moved?.Invoke(Position);
        }
    }

}