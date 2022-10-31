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

        [SerializeField] private float _moveSmoothDuration;

        [Space]

        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;

        private IGrid _grid;

        private Vector3 _position;

        public Vector3 Position => _position;

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
        }

        public void Move(Vector3 direction)
        {
            _position += direction;

            _position = new Vector3(Mathf.Clamp(_position.x, -1, _grid.Size.x - 1), _position.y, Mathf.Clamp(_position.z, -1, _grid.Size.y - 1));

            _transform.DOMove(_grid.GetWorldPosition(_position), _moveSmoothDuration);

            Moved.Invoke(Position);
        }
    }
}