using System;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Infrastructure.Assets;
using VContainer;

namespace _Game.Code.Providers
{
    public class BonusesConfigurationProvider
    {
        public BonusesConfiguration BonusesConfiguration { get; }

        [Inject]
        public BonusesConfigurationProvider(AssetLoadService assetLoadService)
        {
            if (assetLoadService == null) 
                throw  new ArgumentNullException(nameof(assetLoadService));

            BonusesConfiguration = assetLoadService.LoadBonusesConfiguration();
        }
    }
}