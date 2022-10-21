using UnityEngine;
using Zenject;

namespace Sources.GridLogic
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridView _grid;

        public override void InstallBindings()
        {
            Container.Bind<IGrid>().FromInstance(_grid).AsSingle();
        }
    }
}