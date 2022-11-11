using System.Collections;
using Sources.Services;
using UnityEngine;
using Zenject;


namespace Sources.Zenject
{
    public class CoroutineRunnerInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            Container.Bind<ICoroutineRunner>().FromInstance(this).AsSingle();
        }
        public void Invoke(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
    }

}
