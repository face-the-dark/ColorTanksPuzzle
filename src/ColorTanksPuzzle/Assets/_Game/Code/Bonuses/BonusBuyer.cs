using System;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.PersistenceProgress;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class BonusBuyer
    {
        private readonly PlayerDataService _playerDataService;
        private readonly AssetLoadService _assetLoadService;

        private BonusesConfiguration _bonusesConfiguration;

        [Inject]
        public BonusBuyer(PlayerDataService playerDataService, AssetLoadService assetLoadService)
        {
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));
            _assetLoadService = assetLoadService ?? throw new ArgumentNullException(nameof(assetLoadService));
        }

        public void Initialize()
        {
            _bonusesConfiguration = _assetLoadService.LoadBonusesConfiguration();
        }

        public bool TryBuy(Bonus bonus)
        {
            if (TryDeductMoney(bonus))
            {
                IncreaseBonusCount(bonus);

                return true;
            }

            return false;
        }

        private bool TryDeductMoney(Bonus bonus)
        {
            BonusConfiguration bonusConfiguration = _bonusesConfiguration.GetBonusConfiguration(bonus);

            return _playerDataService.TryDeductMoney(bonusConfiguration.Cost);
        }

        private void IncreaseBonusCount(Bonus bonus)
        {
            int count = _bonusesConfiguration.GetBonusConfiguration(bonus).Count;

            _playerDataService.IncreaseBonusCount(bonus, count);
        }
    }
}