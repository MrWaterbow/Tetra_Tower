using DG.Tweening;
using Sources.BuildingLogic;
using Sources.StateMachines;
using System;
using UnityEngine;

namespace Sources.BlockLogic
{
    public enum BlockState
    {
        Placing,
        Placed,
        Instable,
        Falling
    }

    public class BlockView : MonoBehaviour, IBlock
    {
        public event Action<Vector3> Transforming;

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

        private StateMachine<BlockState> _stateMachine;

        private BuildingRoot _buildingRoot;

        private Vector3Int _position;

        private bool _instable;

        public Vector3Int[] Size => _size;
        public Vector3 VisualizationOffset => _visualizationOffset;

        public Vector3Int Position => _position;

        public IReadonlyStateMachine<BlockState> StateMachine => _stateMachine;

        public Transform Transform => _transform;
        public Transform ModelTransform => _modelTransform;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshFilter MeshFilter => _meshFilter;

        public bool Instable => _instable;

        private void OnValidate()
        {
            _hideTime = Mathf.Clamp(_hideTime, 0, float.MaxValue);

            if (_gameObject == null)
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

        public void Initialize(Vector3Int position, BuildingRoot buildingInstaller)
        {
            _stateMachine = new StateMachine<BlockState>(BlockState.Placing);
            _buildingRoot = buildingInstaller;

            _position = position;
        }

        /// <summary>
        /// Fall block.
        /// </summary>
        public void Fall()
        {
            if(_stateMachine.CurrentState == BlockState.Placing)
            {
                _position.y--;

                OnMoving();
            }
        }

        /// <summary>
        /// Move the block to the direction.
        /// </summary>
        /// <param name="direction">moving direction</param>
        public void Move(Vector3Int direction)
        {
            if (_buildingRoot.CheckMovingDirection(this, direction) == false) return;

            _position += direction;

            OnMoving();
        }

        public void Rotate(Vector3 direction, int degree)
        {
            // OLD
            //_transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
            //Transforming?.Invoke(Position);
        }

        /// <summary>
        /// Update the transform position, Checking join and invoke events.
        /// </summary>
        private void OnMoving()
        {
            _transform.DOMove(_buildingRoot.Grid.GetWorldPosition(_position), _buildingRoot.MoveSmooth);

            Transforming?.Invoke(Position);

            if (_buildingRoot.CheckPlaced(this))
            {
                _stateMachine.SetState(BlockState.Placed);
            }
        }

        /// <summary>
        /// Invoke the rigidbody.
        /// </summary>
        public void InvokeRigidbody()
        {
            _rigidbody.isKinematic = false;
        }

        /// <summary>
        /// Invoke all actions to destroy the block.
        /// </summary>
        public void Destroy()
        {
            _meshRenderer.material.DOKill();

            InvokeRigidbody();

            Invoke(nameof(DestroyAnimation), _buildingRoot.MoveSmooth);
        }

        /// <summary>
        /// Invoke destroy animation and destroy block on complete.
        /// </summary>
        private void DestroyAnimation()
        {
            _meshRenderer.material.DOFade(0, _hideTime).onComplete += () =>
            {
                _transform.DOKill();

                Destroy(_gameObject);
            };
        }

        /// <summary>
        /// Set block instable and active tween.
        /// </summary>
        public void InvokeInstable()
        {
            DOInstableColor();

            _instable = true;
        }

        /// <summary>
        /// Part of the instable tween.
        /// </summary>
        private void DOInstableColor()
        {
            _meshRenderer.material.DOColor(_instableColor, _instableAnimationTime).onComplete += DODefaultColor;
        }

        /// <summary>
        /// Part of the instable tween.
        /// </summary>
        private void DODefaultColor()
        {
            _meshRenderer.material.DOColor(_defaultColor, _instableAnimationTime).onComplete += DOInstableColor;
        }
    }
}