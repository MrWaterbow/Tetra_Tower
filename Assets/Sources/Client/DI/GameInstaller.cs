using Client.AsteroidLogic;
using Client.BrickLogic;
using Client.GhostLogic;
using Server.AsteroidLogic;
using Server.BrickLogic;
using Server.Factories;
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
        [SerializeField] private AsteroidView _asteroidPrefab;
        [SerializeField] private GhostView _ghostPrefab;

        [Space]

        [SerializeField] private BrickBootstrapper _brickBootstrapper;

        [Space]

        [SerializeField] private Vector3Int _startPosition;
        [SerializeField] private Vector2Int _surfaceSize;
        [SerializeField] private Transform _worldPositionAnchor;
        [SerializeField] private Transform _asteroidSpawnPoint;

        [Space]

        [SerializeField] private ButtonsBrickInput _brickInput;

        [Space]

        [SerializeField] private Vector3Int[] _asteroidDestroyArea;
        [SerializeField] private float _asteroidFlyTimer;

        private Vector3 _worldPositionOffset;

        public override void InstallBindings()
        {
            // Создание платформы и базы данных
            PlacingSurface placingSurface = new(_surfaceSize, _worldPositionOffset);
            BricksDatabase bricksDatabase = new(placingSurface);

            // Создание фабрики для блока
            IBrickFactory brickFactory = new RandomPatternBrickFactory(BrickBlanks.AllPatterns, _startPosition, bricksDatabase);
            IAsteroidFactory asteroidFactory = new AsteroidFactory(_asteroidDestroyArea, _asteroidFlyTimer);

            // Создание фабрики для визуального блока
            IBrickViewFactory brickViewFactory = new BrickViewFactory(_brickPrefab);
            IAsteroidViewFactory asteroidViewFactory = new AsteroidViewFactory(_asteroidPrefab, _asteroidSpawnPoint);

            // Создание фабрики для призрака
            IGhostViewFactory ghostViewFactory = new GhostViewFactory(_ghostPrefab);

            // База данных для астероидов
            AsteroidsDatabase asteroidsDatabase = new(bricksDatabase, asteroidFactory);

            // Создание модулей для работы с базой данных
            BrickMovementWrapper brickMovementWrapper = new(bricksDatabase);
            BricksRotatingWrapper brickRotatingWrapper = new(bricksDatabase);
            BricksDatabaseAccess bricksDatabaseAccess = new(bricksDatabase);
            BricksCrashWrapper bricksCrashWrapper = new(bricksDatabase);
            AsteroidWrapper asteroidWrapper = new(asteroidsDatabase);

            bricksCrashWrapper.SetCallbacks();

            // Создание презентера для кнопок управления
            IBrickInputPresenter brickInputPresenter = new BrickInputPresenter(brickMovementWrapper, brickRotatingWrapper);

            Container.Bind<IReadOnlyBricksDatabase>().FromInstance(bricksDatabase).AsSingle();
            Container.Bind<IBrickFactory>().FromInstance(brickFactory).AsSingle();
            Container.Bind<IAsteroidFactory>().FromInstance(asteroidFactory).AsSingle();
            Container.Bind<IBrickViewFactory>().FromInstance(brickViewFactory).AsSingle();
            Container.Bind<IAsteroidViewFactory>().FromInstance(asteroidViewFactory).AsSingle();
            Container.Bind<IGhostViewFactory>().FromInstance(ghostViewFactory).AsSingle();
            Container.Bind<BrickMovementWrapper>().FromInstance(brickMovementWrapper).AsSingle();
            Container.Bind<BricksDatabaseAccess>().FromInstance(bricksDatabaseAccess).AsSingle();
            Container.Bind<BricksCrashWrapper>().FromInstance(bricksCrashWrapper).AsSingle();
            Container.Bind<AsteroidWrapper>().FromInstance(asteroidWrapper).AsSingle();
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