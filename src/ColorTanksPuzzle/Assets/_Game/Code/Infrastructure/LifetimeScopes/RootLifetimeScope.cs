using _Game.Code.Infrastructure.Assets;
using _Game.Code.Providers;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadService>(Lifetime.Singleton);
            builder.Register<DifficultyConfigurationProvider>(Lifetime.Singleton);
            builder.Register<LevelConfigurationProvider>(Lifetime.Singleton);
            builder.Register<SceneLoader>(Lifetime.Singleton);
        }
    }
}