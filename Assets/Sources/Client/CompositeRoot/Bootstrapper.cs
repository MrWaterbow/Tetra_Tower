using UnityEngine;

namespace Client.Bootstrapper
{
    internal abstract class Bootstrapper : MonoBehaviour
    {
        /// <summary>
        /// Запускает
        /// </summary>
        public abstract void Boot();
    }
}