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
            _tankDispatcher = tankDispatcher ?? throw new ArgumentNullException(nameof(tankDispatcher));

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

        public void ActivateBonus(Bonus bonus)
        {
            if (_isAllBonusesBlocked == false)
            {
                _bonuses.TryGetValue(bonus, out IBonus activeBonus);

                if (activeBonus == null)
                    throw new InvalidOperationException(nameof(activeBonus));

                activeBonus.Activated += OnActivatedBonus;
                
                _tankDispatcher.Block();
                BlockAllBonuses();
                
                activeBonus.Activate();
            }
        }

        private void ActivateExpansionBonus() => 
            ActivateBonus(Bonus.Expansion);

        private void ActivateFreezeSplineBonus() => 
            ActivateBonus(Bonus.FreezeSpline);

        private void ActivateSacrificeBonus() => 
            ActivateBonus(Bonus.Sacrifice);

        private void ActivateColorRocketBonus() => 
            ActivateBonus(Bonus.ColorRocket);

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