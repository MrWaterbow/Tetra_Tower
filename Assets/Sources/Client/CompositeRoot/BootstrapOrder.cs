using UnityEngine;

namespace UnityIntegration.Bootstrap
{
    internal sealed class BootstrapOrder : MonoBehaviour
    {
        [SerializeField] private Bootstrapper[] _order;

        private void Awake()
        {
            foreach (Bootstrapper bootstrapper in _order)
            {
                bootstrapper.Boot();
            }
        }
    }
}