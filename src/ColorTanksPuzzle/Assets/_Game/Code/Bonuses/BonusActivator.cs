using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.PersistenceProgress;
using _Game.Code.Players;
using _Game.Code.UI;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class BonusActivator : IDisposable
    {
        private readonly InputReader _inputReader;
        private readonly TankDispatcher _tankDispatcher;
        private readonly PlayerDataService _playerDataService;
        private readonly BuyingWindow _buyingWindow;

        private readonly Dictionary<Bonus, IBonus> _bonuses;
        
        private PlayerData _playerData;
        private bool _isAllBonusesBlocked;

        [Inject]
        public BonusActivator
        (
            InputReader inputReader,
            TankDispatcher tankDispatcher,
            BonusBuyer bonusBuyer,
            IReadOnlyList<IBonus> bonuses,
            PlayerDataService playerDataService,
            BuyingWindow buyingWindow
        )
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _tankDispatcher = tankDispatcher ?? throw new ArgumentNullException(nameof(tankDispatcher));
            _playerDataService = playerDataService 
                                            ?? throw new ArgumentNullException(nameof(playerDataService));
            _buyingWindow = buyingWindow ?? throw new ArgumentNullException(nameof(buyingWindow));

            if (bonuses is null || bonuses.Count <= 0)
                throw new ArgumentNullException(nameof(bonuses));

            _bonuses = bonuses.ToDictionary(k => k.Bonus, bonus => bonus);
            
            _inputReader.ExpansionBonusUsed += ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed += ActivateFreezeSplineBonus;
            _inputReader.SacrificeBonusUsed += ActivateSacrificeBonus;
            _inputReader.ColorRocketBonusUsed += ActivateColorRocketBonus;
        }

        public void Dispose()
        {
            _inputReader.ExpansionBonusUsed -= ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed -= ActivateFreezeSplineBonus;
            _inputReader.SacrificeBonusUsed -= ActivateSacrificeBonus;
            _inputReader.ColorRocketBonusUsed -= ActivateColorRocketBonus;
        }

        private void ActivateExpansionBonus() =>
            ActivateBonus(Bonus.Expansion);

        private void ActivateFreezeSplineBonus() =>
            ActivateBonus(Bonus.FreezeSpline);

        private void ActivateSacrificeBonus() =>
            ActivateBonus(Bonus.Sacrifice);

        private void ActivateColorRocketBonus() =>
            ActivateBonus(Bonus.ColorRocket);

        public void ActivateBonus(Bonus bonus)
        {
            if (_playerDataService.IsEnoughBonusCount(bonus) == false)
            {
                ShowBuyingWindow(bonus);
            }
            else if (_isAllBonusesBlocked == false)
            {
                _playerDataService.DecreaseBonusCount(bonus, 1);
                
                _bonuses.TryGetValue(bonus, out IBonus activeBonus);

                if (activeBonus == null)
                    throw new InvalidOperationException(nameof(activeBonus));

                activeBonus.Activated += OnActivatedBonus;

                _tankDispatcher.Block();
                BlockAllBonuses();

                activeBonus.Activate();
            }
        }

        private void ShowBuyingWindow(Bonus bonus) =>
            _buyingWindow.Open(bonus);

        private void OnActivatedBonus(IBonus bonus)
        {
            _tankDispatcher.Unblock();
            UnblockAllBonuses();

            bonus.Activated -= OnActivatedBonus;
        }

        private void BlockAllBonuses() =>
            _isAllBonusesBlocked = true;

        private void UnblockAllBonuses() =>
            _isAllBonusesBlocked = false;
    }
}