using Sources.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace Sources.Scenes
{
    public class SceneLoader
    {
        public event Action<UnityAction> OnLoading;
        public event Action OnLoadingComplete;

        private ICoroutineRunner _coroutineRunner;
        private string _sceneName;
        private float _loadingProgress;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public float LoadingProgress => _loadingProgress;

        public void Load(string sceneName)
        {
            //OnLoading?.Invoke(ScreenShowingComplete);
            _sceneName = sceneName;
        }

        private IEnumerator LoadScene(string sceneName, Action loadingComplete = null)
        {
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            while(loadingOperation.isDone == false)
            {
                _loadingProgress = loadingOperation.progress;
                yield return null;
            }
            loadingComplete?.Invoke();
        }
    }
}
