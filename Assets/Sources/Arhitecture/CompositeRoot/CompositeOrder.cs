using UnityEngine;

namespace Sources.CompositeRootLogic
{
    public class CompositeOrder : MonoBehaviour
    {
        [SerializeField] private CompositeRoot[] _order;

        private void Awake()
        {
            foreach (CompositeRoot compositeRoot in _order)
            {
                compositeRoot.Init();
            }
        }
    }
}
