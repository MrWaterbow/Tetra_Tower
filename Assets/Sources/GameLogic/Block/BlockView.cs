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

        [Space]

        [SerializeField] private float _moveSmoothDuration;

        [Space]

        [SerializeField] private Transform _transform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        private BuildingInstaller _buildingInstaller;

        private Vector3 _position;

        public Vector3Int[] Size => _size;

        public Vector3 Position => _position;

        public Transform Transform => _transform;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshFilter MeshFilter => _meshFilter;

        private void OnValidate()
        {
            _moveSmoothDuration = Mathf.Clamp(_moveSmoothDuration, 0, float.MaxValue);

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
            foreach (Vector3Int size in _size)
            {
                Gizmos.color = new Color(0, 0, 0, 0.4f);

                Gizmos.DrawCube(_transform.position + size, Vector3.one * 1.1f);
            }
        }

        public void Initialize(Vector3 position, BuildingInstaller buildingInstaller)
        {
            _position = position;

            _buildingInstaller = buildingInstaller;
        }

        public void Fall()
        {
            if (_position.y == 0) return;

            _position.y--;

            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position, Vector3.zero), _moveSmoothDuration);

            CheckGrounded();

            Moved?.Invoke(Position);
        }

        public void Move(Vector3 direction)
        {
            if (CanMove(direction) == false) return;

            _position += direction;

            _position = new Vector3(Mathf.Clamp(_position.x, -1, _buildingInstaller.Grid.Size.x - 1), _position.y, Mathf.Clamp(_position.z, -1, _buildingInstaller.Grid.Size.y - 1));

            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position, Vector3.zero), _moveSmoothDuration);

            CheckGrounded();

            Moved?.Invoke(Position);
        }

        private bool CanMove(Vector3 direction)
        {
            if (_position.y == 0) return false;

            return true;
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