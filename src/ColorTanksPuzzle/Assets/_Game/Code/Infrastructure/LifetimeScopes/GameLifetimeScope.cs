using _Game.Code.Bonuses;
using _Game.Code.Cameras;
using _Game.Code.Configurations;
using _Game.Code.Destroyers;
using _Game.Code.Generators.Tanks;
using _Game.Code.Players;
using _Game.Code.Providers;
using _Game.Code.Spawners;
using _Game.Code.SplineModifiers;
using _Game.Code.UI;
using _Game.Code.UI.BonusButtons;
using _Game.Code.Utilities;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Scene Dependencies")]
        [SerializeField] private Transform _pixelArtContainer;
        [SerializeField] private WaitingAreaSpawnBoundaries _waitingAreaSpawnBoundaries;
        [SerializeField] private ExpansionBonusButton _expansionBonusButton;
        [SerializeField] private FreezeSplineBonusButton _freezeSplineBonusButton;
        [SerializeField] private SacrificeBonusButton _sacrificeSplineBonusButton;
        [SerializeField] private Camera _camera;
        [SerializeField] private SplineTanksCountView _splineTanksCountView;
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private SpawningLanesContainer _spawningLanesContainer;
        [SerializeField] private ZoomPoints _zoomPoints;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_expansionBonusButton);
            builder.RegisterComponent(_freezeSplineBonusButton);
            builder.RegisterComponent(_sacrificeSplineBonusButton);
            builder.RegisterComponent(_splineTanksCountView);
            builder.RegisterComponent(_waitingAreaSpawnBoundaries);
            builder.RegisterComponent(_spawningLanesContainer);
            builder.RegisterComponent(_zoomPoints);

            builder.Register<DeterministicRandom>(Lifetime.Singleton);
            builder.Register<PixelArtSpawner>(Lifetime.Singleton)
                .WithParameter(_pixelArtContainer);
            builder.Register<TankHpGenerator>(Lifetime.Singleton);
            builder.Register<TankOrderGenerator>(Lifetime.Singleton);
            builder.Register<TankLaneDistributor>(Lifetime.Singleton)
                .WithParameter(_spawningLanesContainer);
            builder.Register<TanksDataGenerator>(Lifetime.Singleton);
            builder.Register<WaitingArea>(Lifetime.Singleton);
            builder.Register<PlayerInput>(Lifetime.Singleton);
            builder.Register<InputReader>(Lifetime.Singleton);
            builder.Register<LastCircleFinalizer>(Lifetime.Singleton);
            builder.Register<BonusActivator>(Lifetime.Singleton);
            builder.Register<TankDestroyer>(Lifetime.Singleton);
            builder.Register<PixelDestroyer>(Lifetime.Singleton);
            builder.Register<CameraMover>(Lifetime.Singleton)
                .WithParameter(_camera)
                .WithParameter(_zoomPoints);

            builder.Register<BonusesConfigurationProvider>(Lifetime.Singleton);
            builder.Register<TankSettingsProvider>(Lifetime.Singleton);
            
            builder.Register<TankDispatcher>(Lifetime.Singleton);
            builder.Register<Selector>(Lifetime.Singleton)
                .WithParameter(_camera);
            builder.Register<WaitingAreaCellSpawner>(Lifetime.Singleton);
            builder.Register<TankSpawner>(Lifetime.Singleton)
                .WithParameter(_spline)
                .WithParameter(_spawningLanesContainer);
            
            builder.Register<IBonus, ExpansionBonus>(Lifetime.Singleton);
            builder.Register<IBonus, FreezeSplineBonus>(Lifetime.Singleton);
            builder.Register<IBonus, SacrificeBonus>(Lifetime.Singleton);
            builder.Register<IBonus, ColorRocketBonus>(Lifetime.Singleton);
            
            builder.RegisterBuildCallback(container => { container.Resolve<LastCircleFinalizer>(); });

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}