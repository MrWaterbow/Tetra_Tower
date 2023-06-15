using Client.BrickLogic;
using Client.GhostLogic;
using Server.BrickLogic;
using Server.Factories;
using Server.GhostLogic;
using UnityEngine;
using Zenject;

namespace Client.DI
{
    /// <summary>
    /// Создает зависимости для игры
    /// </summary>
    internal sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private BrickView _brickPrefab;
        [SerializeField] private GhostView _ghostPrefab;

        [Space]

        [SerializeField] private BrickBootstrapper _brickBootstrapper;

        [Space]

        [SerializeField] private Vector3Int _startPosition;
        [SerializeField] private Vector2Int _surfaceSize;
        [SerializeField] private Transform _worldPositionAnchor;
        [SerializeField] private ButtonsBrickInput _brickInput;

        private Vector3 _worldPositionOffset;

        public override void InstallBindings()
        {
            // Создание платформы и базы данных
            PlacingSurface placingSurface = new(_surfaceSize, _worldPositionOffset);
            BricksDatabase bricksDatabase = new(placingSurface);

            // Создание фабрики для блока
            IBrickFactory brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns, _startPosition, bricksDatabase);
            IBrickViewFactory brickViewFactory = new BrickViewFactory(_brickPrefab);

            // Создание фабрики для призрака
            IGhostViewFactory ghostViewFactory = new GhostViewFactory(_ghostPrefab);

            // Создание модулей для работы с базой данных
            BrickMovementWrapper brickMovementWrapper = new(bricksDatabase);
            BricksDatabaseAccess bricksDatabaseAccess = new(bricksDatabase);

            // Создание презентера для блока
            IBrickViewPresenter brickPresenter = new BrickViewPresenter(bricksDatabase);

            // Создание презентера для призрака
            IGhostViewPresenter ghostPresenter = new GhostViewPresenter(bricksDatabase);

            // Создание презентера для кнопок управления
            IBrickInputPresenter brickInputPresenter = new BrickInputPresenter(brickMovementWrapper);

            Container.Bind<IBrickFactory>().FromInstance(brickFactory).AsSingle();
            Container.Bind<IBrickViewFactory>().FromInstance(brickViewFactory).AsSingle();
            Container.Bind<IGhostViewFactory>().FromInstance(ghostViewFactory).AsSingle();
            Container.Bind<BrickMovementWrapper>().FromInstance(brickMovementWrapper).AsSingle();
            Container.Bind<BricksDatabaseAccess>().FromInstance(bricksDatabaseAccess).AsSingle();
            Container.Bind<IBrickViewPresenter>().FromInstance(brickPresenter).AsSingle();
            Container.Bind<IGhostViewPresenter>().FromInstance(ghostPresenter).AsSingle();
            Container.Bind<IBrickInputPresenter>().FromInstance(brickInputPresenter).AsSingle();
            Container.Bind<IBricksRuntimeData>().FromInstance(_brickBootstrapper).AsSingle();
        }

        private void OnValidate()
        {
            if (_worldPositionAnchor != null)
            {
                _worldPositionOffset = _worldPositionAnchor.position;
            }
            else
            {
                _worldPositionOffset = Vector3.zero;
            }
        }
    }
}