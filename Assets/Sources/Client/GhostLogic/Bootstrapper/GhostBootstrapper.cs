﻿using Client.BootstrapperLogic;
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
        }

        private void CreateGhostCallbacks()
        {
            _instance?.DisposeCallbacks();
            _ghostViewPresenter?.DisposeCallbacks();

            _instance = _ghostViewFactory.Create();
            _instance.SetCallbacks(_ghostViewPresenter);

            _ghostViewPresenter.SetAndInvokeCallbacks();

            ChangeGhostView();
        }

        private void ChangeGhostCallbacks()
        {
            _instance?.DisposeCallbacks();
            _ghostViewPresenter?.DisposeCallbacks();

            _instance.SetCallbacks();

            _ghostViewPresenter.SetAndInvokeCallbacks();

            ChangeGhostView();
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