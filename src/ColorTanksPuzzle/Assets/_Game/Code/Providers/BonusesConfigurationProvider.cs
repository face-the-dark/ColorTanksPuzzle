using System;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Infrastructure.Assets;

namespace _Game.Code.Providers
{
    public class BonusesConfigurationProvider
    {
        private readonly LoadService _loadService;
        
        public BonusesConfiguration BonusesConfiguration { get; }

        public BonusesConfigurationProvider(LoadService loadService)
        {
            _loadService = loadService ?? throw  new ArgumentNullException(nameof(loadService));

            BonusesConfiguration = loadService.LoadBonusesConfiguration();
        }
    }
}