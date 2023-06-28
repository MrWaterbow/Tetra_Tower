using Client.BootstrapperLogic;
using Client.BrickLogic;
using Server.BrickLogic;
using UnityEngine;
using Zenject;

namespace Client.GhostLogic
{
    /// <summary>
    /// Создает и поддерживает призрака для блока.
    /// </summary>
    internal sealed class GhostBootstrapper : Bootstrapper
    {
        [SerializeField] private float _ghostAlpha;

        private GhostView _instance;

        private IReadOnlyBricksDatabase _database;
        private IGhostViewFactory _ghostViewFactory;
        private IBricksRuntimeData _runtimeData;
        private BrickMovementWrapper _brickMovementWrapper;

        [Inject]
        private void Constructor(
            IReadOnlyBricksDatabase database,
            IGhostViewFactory ghostViewFactory,
            IBricksRuntimeData runtimeData,
            BrickMovementWrapper brickMovementWrapper)
        {
            _database = database;
            _ghostViewFactory = ghostViewFactory;
            _runtimeData = runtimeData;
            _brickMovementWrapper = brickMovementWrapper;
        }

        /// <summary>
        /// Запускает призрака.
        /// </summary>
        public override void Boot()
        {
            CreateGhostCallbacks();

            _brickMovementWrapper.OnControllableBrickFall += ChangeGhostCallbacks;
        }

        /// <summary>
        /// Подписывает призрака на ивенты.
        /// </summary>
        private void CreateGhostCallbacks()
        {
            DisposeCallbacks();

            _instance = _ghostViewFactory.Create();
            _instance.SetCallbacks(_database);

            SetPresenterCallbacksAndInvoke();
        }

        /// <summary>
        /// Меняет подписки на обновленный блок.
        /// </summary>
        private void ChangeGhostCallbacks()
        {
            DisposeCallbacks();

            _instance.RefreshTransform();
            _instance.SetCallbacks(_database);

            SetPresenterCallbacksAndInvoke();
        }

        /// <summary>
        /// Назначает презентер и вызывает его.
        /// </summary>
        private void SetPresenterCallbacksAndInvoke()
        {
            ChangeGhostView();
        }

        /// <summary>
        /// Отписывается от презентера.
        /// </summary>
        private void DisposeCallbacks()
        {
            _instance?.DisposeCallbacks();
        }

        /// <summary>
        /// Меняет визуальное отображение блока.
        /// </summary>
        private void ChangeGhostView()
        {
            Color color = _runtimeData.CurrentBrickView.GeneralColor;
            color.a = _ghostAlpha;

            _instance.Initialize(_database.ControllableBrick.Pattern, color);
        }
    }
}