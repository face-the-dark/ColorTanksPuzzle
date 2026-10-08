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

        private readonly BonusesConfiguration _bonusesConfiguration;

        [Inject]
        public BonusBuyer
        (
            PlayerDataService playerDataService,
            LoadService loadService
        )
        {
            _playerDataService = playerDataService
                                            ?? throw new ArgumentNullException(nameof(playerDataService));

            if (loadService == null)
                throw new ArgumentNullException(nameof(loadService));

            _bonusesConfiguration = loadService.LoadBonusesConfiguration();
        }

        public void Buy(Bonus bonus)
        {
            IncreaseBonusCount(bonus);
            DeductMoney(bonus);
        }

        private void IncreaseBonusCount(Bonus bonus)
        {
            int count = _bonusesConfiguration.GetBonusPurchaseConfiguration(bonus).Count;
            
            _playerDataService.IncreaseBonusCount(bonus, count);
        }

        private void DeductMoney(Bonus bonus)
        {
            BonusPurchaseConfiguration bonusPurchaseConfiguration =
                _bonusesConfiguration.GetBonusPurchaseConfiguration(bonus);

            _playerDataService.DeductMoney(bonusPurchaseConfiguration.Cost);
        }
    }
}