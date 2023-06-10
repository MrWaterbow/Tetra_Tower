using Client.BootstrapperLogic;
using Client.BrickLogic;
using Server.BrickLogic;
using Server.Factories;
using Server.GhostLogic;
using Zenject;

namespace Client.GhostLogic
{
    internal sealed class GhostBootstrapper : Bootstrapper
    {
        private GhostView _instance;

        private IGhostViewPresenter _ghostViewPresenter;
        private IGhostViewFactory _ghostViewFactory;
        private IBricksRuntimeData _runtimeData;
        private BricksSpace _bricksSpace;

        [Inject]
        private void Constructor(
            IGhostViewPresenter ghostViewPresenter,
            IGhostViewFactory ghostViewFactory,
            IBricksRuntimeData runtimeData,
            BricksSpace bricksSpace)
        {
            _ghostViewPresenter = ghostViewPresenter;
            _ghostViewFactory = ghostViewFactory;
            _runtimeData = runtimeData;
            _bricksSpace = bricksSpace;
        }

        public override void Boot()
        {
            CreateGhostCallbacks();

            _bricksSpace.OnControllableBrickFall += ChangeGhostCallbacks;
            _bricksSpace.OnControllableBrickFall += ChangeGhostMesh;
        }

        private void CreateGhostCallbacks()
        {
            _instance?.DisposeCallbacks();
            _ghostViewPresenter?.DisposeCallbacks();

            _ghostViewPresenter.SetCallbacks();

            _instance = _ghostViewFactory.Create();
            _instance.SetCallbacks(_ghostViewPresenter);
        }

        private void ChangeGhostCallbacks()
        {
            _instance?.DisposeCallbacks();
            _ghostViewPresenter?.DisposeCallbacks();

            _ghostViewPresenter.SetCallbacks();
            _instance.SetCallbacks();
        }

        private void ChangeGhostMesh()
        {
            _instance.SetMesh(_runtimeData.CurrentBrickView.MeshFilter.mesh);
        }
    }
}