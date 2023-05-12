using UnityEngine;

namespace Sources.CompositeRootLogic
{
    public sealed class CompositeOrder : MonoBehaviour
    {
        [SerializeField] private CompositeRoot[] _order;

        public CompositeOrder(CompositeRoot[] order)
        {
            _order = order;
        }

        private void Awake()
        {
            foreach (CompositeRoot composite in _order)
            {
                composite.Compose();
            }
        }
    }
}