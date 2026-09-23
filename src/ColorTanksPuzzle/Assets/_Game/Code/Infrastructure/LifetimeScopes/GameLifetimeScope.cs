using _Game.Code.Generators;
using _Game.Code.Generators.Tanks;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Providers;
using _Game.Code.Spawners;
using _Game.Code.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _pixelArtContainer;
        [SerializeField] private TankSpawner _tankSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tankSpawner);
            
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

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}