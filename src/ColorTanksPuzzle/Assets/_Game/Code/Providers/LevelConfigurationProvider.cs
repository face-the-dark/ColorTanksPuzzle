using System;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Configurations.Levels;
using VContainer;

namespace _Game.Code.Providers
{
    public class LevelConfigurationProvider
    {
        private readonly DifficultyConfigurationProvider _difficultyConfigurationProvider;

        [Inject]
        public LevelConfigurationProvider(DifficultyConfigurationProvider difficultyConfigurationProvider)
        {
            _difficultyConfigurationProvider = difficultyConfigurationProvider ??
                                               throw new ArgumentNullException(nameof(difficultyConfigurationProvider));
        }

        public LevelConfiguration LevelConfiguration { get; set; }

        public DifficultyConfiguration GetDifficultyConfiguration() => 
            _difficultyConfigurationProvider.Get(LevelConfiguration.DifficultyMode);
    }
}