using _Game.Code.ConfigurationProvider;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Infrastructure.SceneManagement;
using _Game.Code.PersistenceProgress;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IAssetLoadService, ResourcesAssetLoadService>(Lifetime.Singleton);
            
            builder.Register<DifficultyConfigurationProvider>(Lifetime.Singleton);
            builder.Register<LevelConfigurationProvider>(Lifetime.Singleton);
            
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IPlayerRepository, PrefsPlayerRepository>(Lifetime.Singleton);
            builder.Register<IPlayerDataService, PlayerDataService>(Lifetime.Singleton);
        }
    }
}