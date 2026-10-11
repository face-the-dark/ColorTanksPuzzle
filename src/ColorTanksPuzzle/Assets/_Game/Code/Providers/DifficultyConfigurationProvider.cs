using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Infrastructure.Assets;
using VContainer;

namespace _Game.Code.Providers
{
    public class DifficultyConfigurationProvider
    {
        private readonly Dictionary<DifficultyMode, DifficultyConfiguration> _difficultyConfigurations;

        [Inject]
        public DifficultyConfigurationProvider(AssetLoadService assetLoadService)
        {
            if (assetLoadService == null)
                throw new ArgumentNullException(nameof(assetLoadService));
            
            _difficultyConfigurations = assetLoadService.LoadDifficultyConfigurations()
                .ToDictionary(x => x.DifficultyMode, x => x);
        }

        public DifficultyConfiguration Get(DifficultyMode difficultyMode) => 
            _difficultyConfigurations.GetValueOrDefault(difficultyMode);
    }
}