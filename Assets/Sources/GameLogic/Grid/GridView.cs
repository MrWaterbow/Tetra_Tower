using UnityEngine;

namespace Sources.GridLogic
{
    public class GridView : MonoBehaviour, IGrid
    {
        [Header("Properties")]
        [SerializeField] private Vector2Int _size;

        [Space]

        [SerializeField] private Transform _anchor;
        [SerializeField] private Mesh _gridCellMesh;

        public Vector2Int Size => _size;

        private void OnDrawGizmos()
        {
            DrawGizmosGrid();
        }

        private void OnValidate()
        {
            _size = new Vector2Int(Mathf.Clamp(_size.x, 0, int.MaxValue), Mathf.Clamp(_size.y, 0, int.MaxValue));

            if(_anchor == null)
            {
                _anchor = transform;
            }
        }

        public Vector3 GetWorldPosition(Vector3 position)
        {
            return position * _gridCellMesh.bounds.size.magnitude + _anchor.position;
        }

        private void DrawGizmosGrid()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Gizmos.color = (x + y) % 2 == 0 ? Color.green : Color.red;

                    var meshSize = _gridCellMesh.bounds.size;

                    var position = new Vector3(x * meshSize.x, 0, y * meshSize.y);

                    Gizmos.DrawMesh(_gridCellMesh, position + _anchor.position);
                }
            }
        }
    }
}