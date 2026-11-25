using _Game.Code.Bonuses;
using _Game.Code.Cameras;
using _Game.Code.Coins;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Configurations;
using _Game.Code.Destroyers;
using _Game.Code.Generators.Tanks;
using _Game.Code.Players;
using _Game.Code.Spawners;
using _Game.Code.Spawners.Lines;
using _Game.Code.Spawners.Lines.DOTweenImplementation;
using _Game.Code.SplineModifiers;
using _Game.Code.TankComponents;
using _Game.Code.TankComponents.DOTween;
using _Game.Code.UI;
using _Game.Code.UI.BonusButtons;
using _Game.Code.UI.View;
using _Game.Code.UI.Window;
using _Game.Code.UI.Window.GameOverScreens;
using _Game.Code.Utilities;
using _Game.Code.WaitingAreaComponents;
using SplineMesh;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Scene Dependencies")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Spline _spline;
        [SerializeField] private WaitingAreaCellsSpawnBoundaries _waitingAreaCellsSpawnBoundaries;
        [SerializeField] private SpawnLines _spawnLines;
        [SerializeField] private ZoomPoints _zoomPoints;
        [SerializeField] private Transform _pixelArtContainer;
        [SerializeField] private RectTransform _coinUITarget;

        [Header("UI")]
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private LevelLabel _levelLabel;
        [SerializeField] private SplineTanksCountView _splineTanksCountView;

        [Header("UI Buttons")]
        [SerializeField] private ExpansionBonusButton _expansionBonusButton;
        [SerializeField] private SwitchTanksBonusButton _switchTanksBonusButton;
        [SerializeField] private SacrificeBonusButton _sacrificeSplineBonusButton;
        [SerializeField] private ColorRocketBonusButton _colorRocketSplineBonusButton;
        [SerializeField] private SettingsButton _settingsButton;

        [Header("UI Windows")]
        [SerializeField] private BuyingWindow _buyingWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private WinWindow _winWindow;
        [SerializeField] private LoseWindow _loseWindow;
        
        [Header("Debug")]
        [SerializeField] private GameRunner _gameRunner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterGameRunner(builder);

            RegisterSceneComponents(builder);

            RegisterSceneUIComponents(builder);
            RegisterUIButtons(builder);
            RegisterUIWindows(builder);

            RegisterInfrastructure(builder);
            RegisterConfigurationProviders(builder);
            RegisterInput(builder);
            RegisterTankGenerators(builder);
            RegisterSpawners(builder);
            RegisterDestroyers(builder);
            RegisterWaitingArea(builder);
            RegisterCoinsCounter(builder);
            RegisterGameOver(builder);
            RegisterCameraMover(builder);
            RegisterBonuses(builder);
            RegisterBonusServices(builder);
            RegisterUtilities(builder);
            RegisterAnimators(builder);
            RegisterSplineModifiers(builder);

            InjectIntoSpawnLinesDependencies(builder);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameEntryPoint>();
        }

        private void RegisterGameRunner(IContainerBuilder builder)
        {
            builder.RegisterComponent(_gameRunner);
        }

        private void RegisterSceneComponents(IContainerBuilder builder)
        {
            builder.RegisterComponent(_camera);
            builder.RegisterComponent(_spline);
            builder.RegisterComponent(_waitingAreaCellsSpawnBoundaries);
            builder.RegisterComponent(_spawnLines);
            builder.RegisterComponent(_zoomPoints);
        }

        private void RegisterSceneUIComponents(IContainerBuilder builder)
        {
            builder.RegisterComponent(_levelLabel);
            builder.RegisterComponent(_coinsView);
            builder.RegisterComponent(_splineTanksCountView);
        }

        private void RegisterUIWindows(IContainerBuilder builder)
        {
            builder.RegisterComponent(_buyingWindow);
            builder.RegisterComponent(_settingsWindow);
            builder.RegisterComponent(_winWindow);
            builder.RegisterComponent(_loseWindow);
        }

        private void RegisterUIButtons(IContainerBuilder builder)
        {
            builder.RegisterComponent(_expansionBonusButton);
            builder.RegisterComponent(_switchTanksBonusButton);
            builder.RegisterComponent(_sacrificeSplineBonusButton);
            builder.RegisterComponent(_colorRocketSplineBonusButton);
            builder.RegisterComponent(_settingsButton);
        }

        private void RegisterInfrastructure(IContainerBuilder builder)
        {
            builder.Register<IGameFactory, GameFactory>(Lifetime.Singleton);
        }

        private void RegisterConfigurationProviders(IContainerBuilder builder)
        {
            builder.Register<TankConfigurationProvider>(Lifetime.Singleton);
        }

        private void RegisterInput(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton);
            builder.Register<InputReader>(Lifetime.Singleton);
            builder.Register<TankDispatcher>(Lifetime.Singleton);
            builder.Register<Selector>(Lifetime.Singleton);
        }

        private void RegisterTankGenerators(IContainerBuilder builder)
        {
            builder.Register<TankHpGenerator>(Lifetime.Singleton);
            builder.Register<TankOrderGenerator>(Lifetime.Singleton);
            builder.Register<TankLaneDistributor>(Lifetime.Singleton);
            builder.Register<TanksDataGenerator>(Lifetime.Singleton);
        }

        private void RegisterSpawners(IContainerBuilder builder)
        {
            builder.Register<TankSpawner>(Lifetime.Singleton);
            builder.Register<PixelArtSpawner>(Lifetime.Singleton)
                .WithParameter(_pixelArtContainer);
            builder.Register<WaitingAreaCellSpawner>(Lifetime.Singleton);
            builder.Register<CoinSpawner>(Lifetime.Singleton)
                .WithParameter(_coinUITarget);
        }

        private void RegisterDestroyers(IContainerBuilder builder)
        {
            builder.Register<TankDestroyer>(Lifetime.Singleton);
            builder.Register<PixelDestroyer>(Lifetime.Singleton);
        }

        private void RegisterWaitingArea(IContainerBuilder builder)
        {
            builder.Register<WaitingArea>(Lifetime.Singleton);
        }

        private void RegisterCoinsCounter(IContainerBuilder builder)
        {
            builder.Register<CoinsCounter>(Lifetime.Singleton);
        }

        private void RegisterGameOver(IContainerBuilder builder)
        {
            builder.Register<GameOver>(Lifetime.Singleton);
        }

        private void RegisterCameraMover(IContainerBuilder builder)
        {
            builder.Register<CameraMover>(Lifetime.Singleton);
        }

        private void RegisterBonuses(IContainerBuilder builder)
        {
            builder.Register<IBonus, ExpansionBonus>(Lifetime.Singleton);
            builder.Register<IBonus, SwitchTanksBonus>(Lifetime.Singleton);
            builder.Register<IBonus, SacrificeBonus>(Lifetime.Singleton);
            builder.Register<IBonus, ColorRocketBonus>(Lifetime.Singleton);
        }

        private void RegisterBonusServices(IContainerBuilder builder)
        {
            builder.Register<BonusActivator>(Lifetime.Singleton);
            builder.Register<BonusBuyer>(Lifetime.Singleton);
        }

        private void RegisterUtilities(IContainerBuilder builder)
        {
            builder.Register<DeterministicRandom>(Lifetime.Singleton);
        }

        private void RegisterAnimators(IContainerBuilder builder)
        {
            builder.Register<ITankJumpAnimator, DOTweenTankJumpAnimator>(Lifetime.Transient);
            builder.Register<ITankGroupMoveAnimator, DOTweenTankGroupMoveAnimator>(Lifetime.Transient);
        }

        private void RegisterSplineModifiers(IContainerBuilder builder)
        {
            builder.Register<LastCircleFinalizer>(Lifetime.Singleton);
            builder.RegisterBuildCallback(container => { container.Resolve<LastCircleFinalizer>(); });
        }

        private void InjectIntoSpawnLinesDependencies(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                foreach (SpawnLine spawnLine in _spawnLines.Lines) 
                    container.Inject(spawnLine);
            });
        }
    }
}