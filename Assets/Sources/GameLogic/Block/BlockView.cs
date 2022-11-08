using DG.Tweening;
using Sources.GridLogic;
using System;
using UnityEngine;
using Zenject;

namespace Sources.BlockLogic
{
    public class BlockView : MonoBehaviour, IBlock
    {
        public event Action<Vector3> Moved;
        public event Action Placed;

        [SerializeField] private Vector3[] _size;
        [SerializeField] private Color _gizmosColor = Color.white;

        [Space]

        [SerializeField] private float _moveSmoothDuration;

        [Space]

        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _offsetTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        private IGrid _grid;

        private Vector3 _position;

        public Vector3 Position => _position;

        public Transform OffsetTransform => _offsetTransform;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public MeshFilter MeshFilter => _meshFilter;

        private void OnDrawGizmosSelected()
        {
            foreach (Vector3 move in _size)
            {
                Vector2 position = move + _transform.position;

                Gizmos.DrawCube(position, Vector3.one * 3);
            }

            //if (_moves == null || _attackMoves == null) return;

            //foreach (Vector2 move in _moves)
            //{
            //    Vector2 position = move * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

            //    if (_attackMoves.Any(attackMove => attackMove == move))
            //    {
            //        Gizmos.color = _universalColor;

            //        Gizmos.DrawWireCube(position, _transform.localScale);

            //        continue;
            //    }

            //    Gizmos.color = _moveColor;

            //    Gizmos.DrawWireCube(position, _transform.localScale);
            //}

            //Gizmos.color = _attackColor;

            //foreach (Vector2 attackMove in _attackMoves)
            //{
            //    Vector2 position = attackMove * _transform.localScale * _spacing + (Vector2)_transform.localPosition;

            //    if (_moves.Any(move => move == attackMove)) continue;

            //    Gizmos.DrawWireCube(position, _transform.localScale);
            //}
        }

        private void OnValidate()
        {
            _moveSmoothDuration = Mathf.Clamp(_moveSmoothDuration, 0, float.MaxValue);

            if(_transform == null)
            {
                _transform = transform;
            }

            if(_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }
        }

        public void Initialize(Vector3 position, IGrid grid)
        {
            _position = position;

            _grid = grid;
        }

        public void Fall()
        {
            if (_position.y == 0) return;

            _position.y--;

            _transform.DOMove(_grid.GetWorldPosition(_position), _moveSmoothDuration);

            if(_position.y == 0)
            {
                Placed?.Invoke();
            }
        }

        public void Move(Vector3 direction)
        {
            if (_position.y == 0) return;

            _position += direction;

            _position = new Vector3(Mathf.Clamp(_position.x, -1, _grid.Size.x - 1), _position.y, Mathf.Clamp(_position.z, -1, _grid.Size.y - 1));

            _transform.DOMove(_grid.GetWorldPosition(_position), _moveSmoothDuration);

            Moved?.Invoke(Position);
        }
    }
}