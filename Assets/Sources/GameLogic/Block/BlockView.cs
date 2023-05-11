using DG.Tweening;
using Sources.BuildingLogic;
using Sources.StateMachines;
using System;
using UnityEngine;
using Zenject;

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

        [Space]

        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Rigidbody _rigidbody;

        private StateMachine<BlockState> _stateMachine;

        private BuildingRoot _buildingRoot;
        private BlockVisualization _blockVisualization;

        private Vector3Int _position;
        private Color _instableColor;
        private Color _defaultColor;

        private bool _instable;

        private bool _disableAnimation;

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
        private void Start()
        {
            _blockVisualization = FindObjectOfType<BlockVisualization>();
            _blockVisualization.Rotate(0);
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

            _defaultColor = MeshRenderer.material.color;
            _instableColor = MeshRenderer.material.color - new Color(0.3f, 0.3f, 0.3f, 0);
        }

        /// <summary>
        /// Fall block.
        /// </summary>
        public void Fall()
        {
            if (_stateMachine.CurrentState == BlockState.Placing)
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

        #region Block Rotation functions
        public void Rotate(Vector3 direction, int degree)
        {
            if (_transform.gameObject.name == "O-Block(Clone)")
            {
                OBlockRotate(direction, degree);
            }
            else if (_transform.gameObject.name == "T-Block(Clone)")
            {
                TBlockRotate(direction, degree);
            }
            else if (_transform.gameObject.name == "L-Block(Clone)")
            {
                LBlockRotation(direction, degree);
            }
            // OLD
            // OnMoving();
            // Transforming?.Invoke(Position);
        }

        private void OBlockRotate(Vector3 direction, int degree)
        {
            if (direction == Vector3.up)
            {
                return;
            }
            else
            {
                //_transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
            }
        }

        private Vector3Int[] _tposesX =
        {
            new Vector3Int(0,0,1),
            new Vector3Int(1,0,0),
            new Vector3Int(0,0,-1),
            new Vector3Int(-1,0,0),
        };
        private void TBlockRotate(Vector3 direction, int degree)
        {
            if (direction == Vector3.up)
            {
                if (_size[1] == _tposesX[0])
                {
                    if (degree > 0)
                    {
                        _size[3] = _tposesX[0];
                        _size[1] = _tposesX[1];
                        _size[2] = _tposesX[2];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(90);
                    }
                    else
                    {
                        _size[3] = _tposesX[2];
                        _size[1] = _tposesX[3];
                        _size[2] = _tposesX[0];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(-90);
                    }
                }
                else if (_size[1] == _tposesX[1])
                {
                    if (degree > 0)
                    {
                        _size[1] = _tposesX[2];
                        _size[2] = _tposesX[3];
                        _size[3] = _tposesX[1];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(180);
                    }
                    else
                    {
                        _size[1] = _tposesX[0];
                        _size[2] = _tposesX[1];
                        _size[3] = _tposesX[3];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(0);

                    }
                }
                else if (_size[1] == _tposesX[2])
                {
                    if (degree > 0)
                    {
                        _size[1] = _tposesX[3];
                        _size[2] = _tposesX[0];
                        _size[3] = _tposesX[2];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(270);
                    }
                    else
                    {
                        _size[1] = _tposesX[1];
                        _size[2] = _tposesX[2];
                        _size[3] = _tposesX[0];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(90);
                    }
                }
                else if (_size[1] == _tposesX[3])
                {
                    if (degree > 0)
                    {
                        _size[1] = _tposesX[0];
                        _size[2] = _tposesX[1];
                        _size[3] = _tposesX[3];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(0);
                    }
                    else
                    {
                        _size[1] = _tposesX[2];
                        _size[2] = _tposesX[3];
                        _size[3] = _tposesX[1];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(180);
                    }
                }
            }
            else
            {

            }
        }

        private Vector3Int[] _lposesX =
        {
            new Vector3Int(-1,0,1),
            new Vector3Int(0,0,1),
            new Vector3Int(1,0,1),
            new Vector3Int(-1,0,0),
            new Vector3Int(1,0,0),
            new Vector3Int(-1,0,-1),
            new Vector3Int(0,0,-1),
            new Vector3Int(1,0,-1),
        };

        private void LBlockRotation(Vector3 direction, int degree)
        {
            if (direction == Vector3.up)
            { 
                if(_size[1] == _lposesX[1])
                {
                    if (degree > 0)
                    {
                        _size[1] = _lposesX[4];
                        _size[2] = _lposesX[3];
                        _size[3] = _lposesX[5];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(90);
                    }
                    else
                    {
                        _size[1] = _lposesX[3];
                        _size[2] = _lposesX[4];
                        _size[3] = _lposesX[2];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(-90);
                    }
                }
                else if (_size[1] == _lposesX[4])
                {
                    if (degree > 0)
                    {
                        _size[1] = _lposesX[6];
                        _size[2] = _lposesX[1];
                        _size[3] = _lposesX[0];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(180);
                    }
                    else
                    {
                        _size[1] = _lposesX[1];
                        _size[2] = _lposesX[6];
                        _size[3] = _lposesX[7];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(0);
                    }
                }
                else if (_size[1] == _lposesX[6])
                {
                    if (degree > 0)
                    {
                        _size[1] = _lposesX[3];
                        _size[2] = _lposesX[4];
                        _size[3] = _lposesX[2];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(270);
                    }
                    else
                    {
                        _size[1] = _lposesX[4];
                        _size[2] = _lposesX[3];
                        _size[3] = _lposesX[5];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(90);
                    }
                }
                else if (_size[1] == _lposesX[3])
                {
                    if (degree > 0)
                    {
                        _size[1] = _lposesX[1];
                        _size[2] = _lposesX[6];
                        _size[3] = _lposesX[7];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(0);
                    }
                    else
                    {
                        _size[1] = _lposesX[6];
                        _size[2] = _lposesX[1];
                        _size[3] = _lposesX[0];
                        _transform.RotateAround(transform.GetChild(0).gameObject.transform.position, direction, degree);
                        _blockVisualization.Rotate(180);
                    }
                }
            }
            else
            {

            }
        }


        #endregion
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
            if (_instable) return;

            _instable = true;

            DOInstableColor();

        }

        /// <summary>
        /// Part of the instable tween.
        /// </summary>
        private void DOInstableColor()
        {
            _meshRenderer.material.DOColor(_instableColor, _instableAnimationTime).onComplete += () =>
            {
                if (_disableAnimation == false)
                {
                    DODefaultColor();
                }
                else
                {
                    _disableAnimation = false;

                    DeactiveInstableColor();
                }
            };
        }

        /// <summary>
        /// Part of the instable tween.
        /// </summary>
        private void DODefaultColor()
        {
            _meshRenderer.material.DOColor(_defaultColor, _instableAnimationTime).onComplete += () =>
            {
                if (_disableAnimation == false)
                {
                    DOInstableColor();
                }
                else
                {
                    _disableAnimation = false;

                    DeactiveInstableColor();
                }
            };
        }

        private void DeactiveInstableColor()
        {
            _meshRenderer.material.DOColor(_defaultColor, _instableAnimationTime);
        }

        public void DeinvokeInstable()
        {
            if (_instable == false) return;

            _instable = false;
            _disableAnimation = true;

            _meshRenderer.material.DOKill(true);

            DeactiveInstableColor();
        }

        public void SetCenterOfMass(Vector3 offset)
        {
            _rigidbody.centerOfMass = offset;
        }
    }
}