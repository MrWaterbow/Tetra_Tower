using UnityEngine;
using Zenject;

namespace Sources.GridLogic
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridView _grid;

        private void OnValidate()
        {
            if(_grid == null)
            {
                _grid = FindObjectOfType<GridView>();
            }
        }

        public override void InstallBindings()
        {
            Container.Bind<IGrid>().FromInstance(_grid).AsSingle();
        }
    }
}