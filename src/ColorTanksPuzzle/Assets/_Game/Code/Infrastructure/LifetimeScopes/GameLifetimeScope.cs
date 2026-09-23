using _Game.Code.Configurations;
using _Game.Code.Generators;
using _Game.Code.Generators.Tanks;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Players;
using _Game.Code.Providers;
using _Game.Code.Spawners;
using _Game.Code.Utilities;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _pixelArtContainer;
        [SerializeField] private TankSpawner _tankSpawner;
        [SerializeField] private TankDispatcher _tankDispatcher;
        [SerializeField] private WaitingAreaSpawnBoundaries _waitingAreaSpawnBoundaries;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tankSpawner);
            builder.RegisterComponent(_tankDispatcher);
            
            builder.Register<LoadService>(Lifetime.Singleton);
            builder.Register<DeterministicRandom>(Lifetime.Singleton);
            builder.Register<DifficultyConfigurationProvider>(Lifetime.Singleton);
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
            builder.Register<BonusActivator>(Lifetime.Singleton);
            builder.RegisterBuildCallback(container =>
            {
                container.Resolve<BonusActivator>();
            });
            builder.Register<WaitingAreaCellSpawner>(Lifetime.Singleton)
                .WithParameter(_waitingAreaSpawnBoundaries);

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}