using UnityEngine;

namespace Client.BootstrapperLogic
{
    internal abstract class Bootstrapper : MonoBehaviour
    {
        /// <summary>
        /// ���������
        /// </summary>
        public abstract void Boot();
    }
}