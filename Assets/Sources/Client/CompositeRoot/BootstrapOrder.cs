using UnityEngine;

namespace Client.Bootstrapper
{
    internal sealed class BootstrapOrder : MonoBehaviour
    {
        /// <summary>
        /// Порядок запуска
        /// </summary>
        [SerializeField] private Bootstrapper[] _order;

        private void Awake()
        {
            // Вызывает всё по очереди

            foreach (Bootstrapper bootstrapper in _order)
            {
                bootstrapper.Boot();
            }
        }
    }
}