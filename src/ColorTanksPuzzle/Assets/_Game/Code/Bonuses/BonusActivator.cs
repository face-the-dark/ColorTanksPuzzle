using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Players;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class BonusActivator : IDisposable
    {
        private readonly InputReader _inputReader;
        private readonly TankDispatcher _tankDispatcher;

        private readonly Dictionary<Bonus, IBonus> _bonuses;

        private bool _isAllBonusesBlocked;

        [Inject]
        public BonusActivator(InputReader inputReader , TankDispatcher tankDispatcher, IReadOnlyList<IBonus> bonuses)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _tankDispatcher = tankDispatcher;

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

        public void ActivateBonus(Bonus bonus)
        {
            if (_isAllBonusesBlocked == false)
            {
                _bonuses.TryGetValue(bonus, out IBonus activeBonus);

                if (activeBonus == null)
                    throw new InvalidOperationException(nameof(activeBonus));

                activeBonus.Activate();
            }
        }

        private void ActivateExpansionBonus() => 
            ActivateBonus(Bonus.Expansion);

        private void ActivateFreezeSplineBonus() => 
            ActivateBonus(Bonus.FreezeSpline);

        private void ActivateSacrificeBonus()
        {
            ActivateBonus(Bonus.Sacrifice);
            
            _tankDispatcher.Block();
            BlockAllBonuses();

            IBonus sacrificeBonus = _bonuses[Bonus.Sacrifice];

            sacrificeBonus.Activated += OnActivatedBonus;
        }
        
        private void ActivateColorRocketBonus()
        {
            ActivateBonus(Bonus.ColorRocket);
            
            _tankDispatcher.Block();
            BlockAllBonuses();

            IBonus sacrificeBonus = _bonuses[Bonus.ColorRocket];

            sacrificeBonus.Activated += OnActivatedBonus;
        }

        private void OnActivatedBonus(IBonus bonus)
        {
            bonus.Activated -= OnActivatedBonus;
            
            _tankDispatcher.Unblock();
            UnblockAllBonuses();
        }
        
        private void BlockAllBonuses() => 
            _isAllBonusesBlocked = true;

        private void UnblockAllBonuses() => 
            _isAllBonusesBlocked = false;
    }
}