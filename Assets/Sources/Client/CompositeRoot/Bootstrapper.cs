using UnityEngine;

namespace Client.Bootstrapper
{
    internal abstract class Bootstrapper : MonoBehaviour
    {
        /// <summary>
        /// ���������
        /// </summary>
        public abstract void Boot();
    }
}