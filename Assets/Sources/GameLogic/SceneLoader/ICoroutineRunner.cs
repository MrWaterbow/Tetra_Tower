using System.Collections;
using UnityEngine;

namespace Sources.Services
{
    public interface ICoroutineRunner
    {
        public void Invoke(IEnumerator enumerator);
    }
}
