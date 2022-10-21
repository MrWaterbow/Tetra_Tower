using Sources.BlockLogic;
using UnityEngine;
using Zenject;

namespace Sources.BuildingLogic
{
    public class BuildingInstaller : MonoInstaller
    {
        [SerializeField] private int _height;
        [SerializeField] private float _fallTick;

        [SerializeField] private BlockView _startBlock;
        [SerializeField] private BlockView[] _blockList;

        private float _tick;

        private void Update()
        {
            _tick += Time.deltaTime;

            if(_tick >= _fallTick)
            {
                
            }
        }

        public override void InstallBindings()
        {

        }
    }
}