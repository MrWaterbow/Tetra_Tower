using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.SceneLoaderLogic
{
    internal class MonoSceneLoader : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
