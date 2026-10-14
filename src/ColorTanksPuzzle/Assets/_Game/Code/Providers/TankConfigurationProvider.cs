using System;
using _Game.Code.Configurations;
using _Game.Code.Infrastructure.Assets;
using VContainer;

namespace _Game.Code.Providers
{
    public class TankConfigurationProvider
    {
        public TankConfiguration TankConfiguration { get; }

        [Inject]
        public TankConfigurationProvider(AssetLoadService assetLoadService)
        {
            if (assetLoadService == null)
                throw new ArgumentNullException(nameof(assetLoadService));

            TankConfiguration = assetLoadService.LoadTankConfiguration();
        }
    }
}