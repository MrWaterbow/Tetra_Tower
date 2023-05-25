using UnityEngine;

namespace Client.Bootstrapper
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