using UnityEngine;

namespace Sources.GridLogic
{
    public class GridView : MonoBehaviour, IGrid
    {
        [Header("Properties")]
        [SerializeField] private Vector2Int _size;

        [Space]

        [SerializeField] private Transform _anchor;

        public Vector2Int Size => _size;

        private void OnValidate()
        {
            _size = new Vector2Int(Mathf.Clamp(_size.x, 0, int.MaxValue), Mathf.Clamp(_size.y, 0, int.MaxValue));

            if(_anchor == null)
            {
                _anchor = transform;
            }
        }
    }
}