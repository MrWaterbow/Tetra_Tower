using DG.Tweening;
using Lean.Transition;
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

        [SerializeField] private float _hideTime;

        [Space]

        [SerializeField] private float _instableAnimationTime;
        [SerializeField] private Color _instableColor;
        [SerializeField] private Color _defaultColor;

        [Space]

        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Rigidbody _rigidbody;

        private BuildingInstaller _buildingInstaller;

        private Vector3Int _position;

        private bool _instable;

        public Vector3Int[] Size => _size;
        public Vector3 VisualizationOffset => _visualizationOffset;

        public Vector3Int Position => _position;

        public Transform Transform => _transform;
        public Transform ModelTransform => _modelTransform;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshFilter MeshFilter => _meshFilter;

        public bool Instable => _instable;

        private void OnValidate()
        {
            _hideTime = Mathf.Clamp(_hideTime, 0, float.MaxValue);

            if(_gameObject == null)
            {
                _gameObject = gameObject;
            }

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

        public void Rotate(Vector3 direction, int degree)
        {
            _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
            //Moved?.Invoke(Position);
        }

        public void Destroy()
        {
            _meshRenderer.material.DOKill();

            ActivePhysics();

            Invoke(nameof(DestroyAnimation), _buildingInstaller.MoveSmooth);
        }

        private void DestroyAnimation()
        {
            _meshRenderer.material.DOFade(0, _hideTime).onComplete += () =>
            {
                _transform.DOKill();

                Destroy(_gameObject);
            };
        }

        private void OnMoving()
        {
            _transform.DOMove(_buildingInstaller.Grid.GetWorldPosition(_position), _buildingInstaller.MoveSmooth);

            CheckGrounded();

            Moved?.Invoke(Position);
        }

        private bool CanMove(Vector3Int direction)
        {
            return _buildingInstaller.CheckMovingDirection(this, direction);
        }

        private void CheckGrounded()
        {
            if (_buildingInstaller.OnGround(this))
            {
                Placed?.Invoke();
            }
        }

        public void ActivePhysics()
        {
            _rigidbody.isKinematic = false;
        }

        /// <summary>
        /// Скорее всего бесполезная хреня, но пусть будет по рофлу
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRaycast()
        {
            Ray ray = new(_modelTransform.position - new Vector3(0, 0.5f, 0), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.transform.TryGetComponent(out ViewCollider collider))
                {
                    return hit.point;
                }
            }

            return Vector3.zero;
        }

        public void MakeInstable()
        {
            DOInstableColor();

            _instable = true;
        }

        private void DOInstableColor()
        {
            _meshRenderer.material.DOColor(_instableColor, _instableAnimationTime).onComplete += DODefaultColor;
        }
        
        private void DODefaultColor()
        {
            _meshRenderer.material.DOColor(_defaultColor, _instableAnimationTime).onComplete += DOInstableColor;
        }
    }
}