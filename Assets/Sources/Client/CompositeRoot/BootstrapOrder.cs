using UnityEngine;

namespace Client.BootstrapperLogic
{
    internal sealed class BootstrapOrder : MonoBehaviour
    {
        /// <summary>
        /// ������� �������
        /// </summary>
        [SerializeField] private Bootstrapper[] _order;

        private void Awake()
        {
            // �������� �� �� �������

            foreach (Bootstrapper bootstrapper in _order)
            {
                bootstrapper.Boot();
            }
        }
    }
}