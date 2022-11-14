using DG.Tweening;
using Sources.BuildingLogic;
using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public class BlockView : MonoBehaviour, IBlock
    {
        public event Action<Vector3> Moved;
        public event Action Placed;

        [SerializeField] private Vector3[] _size;
        [SerializeField] private bool _halfSize;

        [Space]

        [SerializeField] private float _moveSmoothDuration;

        [Space]

        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _offsetTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        private BuildingInstaller _buildingInstaller;

        private Vector3 _position;

        public Vector3[] Size => _size;
        public bool HalfSize => _halfSize;

        public Vector3 Position => _position;

        public Transform OffsetTransform => _offsetTransform;
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

        public void Initialize(Vector3 position, BuildingInstaller buildingInstaller)
        {
            _position = position;

            _buildingInstaller = buildingInstaller;
        }

        public void Fall()
        {
            if (_position.y == 0) return;

            _position.y--;

            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position, _halfSize ? new Vector3(0.55f, 0, 0.65f) : Vector3.zero), _moveSmoothDuration);

            if (_buildingInstaller.OnGround(this))
            {
                Placed?.Invoke();
            }

            Moved?.Invoke(Position);
        }

        public void Move(Vector3 direction)
        {
            if (_position.y == 0) return;

            _position += direction;

            _position = new Vector3(Mathf.Clamp(_position.x, -1, _buildingInstaller.Grid.Size.x - 1), _position.y, Mathf.Clamp(_position.z, -1, _buildingInstaller.Grid.Size.y - 1));

            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position, _halfSize ? new Vector3(0.55f, 0, 0.65f) : Vector3.zero), _moveSmoothDuration);

            Moved?.Invoke(Position);
        }
    }
}