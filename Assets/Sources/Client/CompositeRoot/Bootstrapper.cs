using UnityEngine;

namespace Client.BootstrapperLogic
{
    internal abstract class Bootstrapper : MonoBehaviour
    {
        /// <summary>
        /// Запускает
        /// </summary>
        public abstract void Boot();
    }
}