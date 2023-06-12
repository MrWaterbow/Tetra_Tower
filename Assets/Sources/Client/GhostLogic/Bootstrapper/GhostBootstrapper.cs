using Client.BootstrapperLogic;
using Client.BrickLogic;
using Server.BrickLogic;
using Server.Factories;
using Server.GhostLogic;
using UnityEngine;
using Zenject;

namespace Client.GhostLogic
{
    internal sealed class GhostBootstrapper : Bootstrapper
    {
        [SerializeField] private float _ghostAlpha;

        private GhostView _instance;

        private IGhostViewPresenter _ghostViewPresenter;
        private IGhostViewFactory _ghostViewFactory;
        private IBricksRuntimeData _runtimeData;
        private BrickMovementWrapper _brickMovementWrapper;

        [Inject]
        private void Constructor(
            IGhostViewPresenter ghostViewPresenter,
            IGhostViewFactory ghostViewFactory,
            IBricksRuntimeData runtimeData,
            BrickMovementWrapper brickMovementWrapper)
        {
            _ghostViewPresenter = ghostViewPresenter;
            _ghostViewFactory = ghostViewFactory;
            _runtimeData = runtimeData;
            _brickMovementWrapper = brickMovementWrapper;
        }

        public override void Boot()
        {
            CreateGhostCallbacks();

            _brickMovementWrapper.OnControllableBrickFall += ChangeGhostCallbacks;
        }

        private void CreateGhostCallbacks()
        {
            DisposeCallbacks();

            _instance = _ghostViewFactory.Create();
            _instance.SetCallbacks(_ghostViewPresenter);

            SetPresenterCallbacksAndInvoke();
        }

        private void ChangeGhostCallbacks()
        {
            DisposeCallbacks();

            _instance.SetCallbacks();

            SetPresenterCallbacksAndInvoke();
        }

        private void SetPresenterCallbacksAndInvoke()
        {
            _ghostViewPresenter.SetAndInvokeCallbacks();

            ChangeGhostView();
        }

        private void DisposeCallbacks()
        {
            _instance?.DisposeCallbacks();
            _ghostViewPresenter?.DisposeCallbacks();
        }

        private void ChangeGhostView()
        {
            ChangeGhostMesh();
            ChangeGhostColor();
        }

        private void ChangeGhostMesh()
        {
            _instance.SetMesh(_runtimeData.CurrentBrickView.GetMesh());
        }

        private void ChangeGhostColor()
        {
            Color color = _runtimeData.CurrentBrickView.GetColor();
            color.a = _ghostAlpha;

            _instance.SetColor(color);
        }
    }
}