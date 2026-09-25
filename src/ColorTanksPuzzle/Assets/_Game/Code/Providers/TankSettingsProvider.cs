using System;
using _Game.Code.Configurations;
using _Game.Code.Infrastructure.Assets;
using VContainer;

namespace _Game.Code.Providers
{
    public class TankSettingsProvider
    {
        public TankSettings TankSettings { get; }

        [Inject]
        public TankSettingsProvider(LoadService loadService)
        {
            if (loadService == null)
                throw new ArgumentNullException(nameof(loadService));

            TankSettings = loadService.LoadTankSettings();
        }
    }
}