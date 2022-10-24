using Sources.GridLogic;
using UnityEngine;
using Zenject;

namespace Sources.BlockLogic
{
    public class BlockView : MonoBehaviour, IBlock
    {
        [SerializeField] private Transform _transform;

        private IGrid _grid;

        private Vector3 _position;

        public Vector3 Position => _position;

        public void Fall()
        {
            _position.y--;

            _transform.position = _grid.GetWorldPosition(_position);
        }

        public void Initialize(Vector3 position, IGrid grid)
        {
            _position = position;

            _grid = grid;
        }

        public void Move(Vector3 direction)
        {
            _position += direction;

            _transform.position = _grid.GetWorldPosition(_position);
        }
    }
}