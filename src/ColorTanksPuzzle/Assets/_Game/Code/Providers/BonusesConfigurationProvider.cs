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
        public BonusesConfigurationProvider(LoadService loadService)
        {
            if (loadService == null) 
                throw  new ArgumentNullException(nameof(loadService));

            BonusesConfiguration = loadService.LoadBonusesConfiguration();
        }
    }
}