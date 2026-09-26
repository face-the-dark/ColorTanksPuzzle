using _Game.Code.Bonuses;
using _Game.Code.Cameras;
using _Game.Code.Configurations;
using _Game.Code.Generators;
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
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Scene Dependencies")]
        [SerializeField] private Transform _pixelArtContainer;
        [SerializeField] private TankSpawner _tankSpawner;
        [SerializeField] private WaitingAreaSpawnBoundaries _waitingAreaSpawnBoundaries;
        [SerializeField] private ExpansionBonusButton _expansionBonusButton;
        [SerializeField] private FreezeSplineBonusButton _freezeSplineBonusButton;
        [SerializeField] private SacrificeBonusButton _sacrificeSplineBonusButton;
        [SerializeField] private Camera _camera;
        [SerializeField] private SplineTanksCountView _splineTanksCountView;
        [SerializeField] private Transform _cameraSeparationPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tankSpawner);
            builder.RegisterComponent(_expansionBonusButton);
            builder.RegisterComponent(_freezeSplineBonusButton);
            builder.RegisterComponent(_sacrificeSplineBonusButton);
            builder.RegisterComponent(_splineTanksCountView);

            builder.Register<DeterministicRandom>(Lifetime.Singleton);
            builder.Register<PixelArtGenerator>(Lifetime.Singleton)
                .WithParameter(_pixelArtContainer);
            builder.Register<TankHpGenerator>(Lifetime.Singleton);
            builder.Register<TankOrderGenerator>(Lifetime.Singleton);
            builder.Register<TankLaneDistributor>(Lifetime.Singleton)
                .WithParameter(_tankSpawner.LanesCount);
            builder.Register<TanksDataGenerator>(Lifetime.Singleton);
            builder.Register<WaitingArea>(Lifetime.Singleton);
            builder.Register<PlayerInput>(Lifetime.Singleton);
            builder.Register<InputReader>(Lifetime.Singleton);
            builder.Register<LastCircleFinalizer>(Lifetime.Singleton);
            builder.Register<BonusActivator>(Lifetime.Singleton);
            builder.Register<CameraMover>(Lifetime.Singleton)
                .WithParameter(_camera)
                .WithParameter(_cameraSeparationPoint);
            
            builder.Register<BonusesConfigurationProvider>(Lifetime.Singleton);
            builder.Register<TankSettingsProvider>(Lifetime.Singleton);
            
            builder.Register<TankDispatcher>(Lifetime.Singleton);
            builder.Register<TankSelector>(Lifetime.Singleton)
                .WithParameter(_camera);
            builder.Register<WaitingAreaCellSpawner>(Lifetime.Singleton)
                .WithParameter(_waitingAreaSpawnBoundaries);
            
            builder.Register<IBonus, ExpansionBonus>(Lifetime.Singleton);
            builder.Register<IBonus, FreezeSplineBonus>(Lifetime.Singleton);
            builder.Register<IBonus, SacrificeBonus>(Lifetime.Singleton);
            
            builder.RegisterBuildCallback(container => { container.Resolve<LastCircleFinalizer>(); });

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}