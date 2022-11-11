using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sources.Scenes;
using Sources.Services;


namespace Sources.Zenject
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        private SceneLoader _sceneLoader;
        private ICoroutineRunner _coroutineRunner;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public override void InstallBindings()
        {
            _sceneLoader = new SceneLoader(_coroutineRunner);
            Container.Bind<SceneLoader>().FromInstance(_sceneLoader).AsSingle();
        }
    }
}
