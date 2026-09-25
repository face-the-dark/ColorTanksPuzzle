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

        [Inject]
        public BonusActivator(InputReader inputReader , TankDispatcher tankDispatcher, IReadOnlyList<IBonus> bonuses)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _tankDispatcher = tankDispatcher;

            _bonuses = bonuses.ToDictionary(k => k.Bonus, bonus => bonus);

            _inputReader.ExpansionBonusUsed += ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed += ActivateFreezeSplineBonus;
            _inputReader.SacrificeBonusUsed += ActivateSacrificeBonus;
        }

        public void Dispose()
        {
            _inputReader.ExpansionBonusUsed -= ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed -= ActivateFreezeSplineBonus;
            _inputReader.SacrificeBonusUsed -= ActivateSacrificeBonus;
        }

        public IBonus ActivateBonus(Bonus bonus)
        {
            _bonuses.TryGetValue(bonus, out IBonus activeBonus);

            if (activeBonus == null)
                throw new InvalidOperationException(nameof(activeBonus));

            activeBonus.Activate();
            
            return activeBonus;
        }

        public void BlockAllBonuses()
        {
            foreach (var bonus in _bonuses.Values)
            {
                bonus.Block();
            }
        }

        public void UnblockAllBonuses()
        {
            foreach (var bonus in _bonuses.Values)
            {
                bonus.Unblock();
            }
        }

        private void ActivateExpansionBonus() => 
            ActivateBonus(Bonus.Expansion);

        private void ActivateFreezeSplineBonus() => 
            ActivateBonus(Bonus.FreezeSpline);

        private void ActivateSacrificeBonus()
        {
            _tankDispatcher.Block();
            
            BlockAllBonuses();

            IBonus activateBonus = ActivateBonus(Bonus.Sacrifice);
            
            activateBonus.Activated += OnActivatedSacrificeBonus;
        }

        private void OnActivatedSacrificeBonus(IBonus bonus)
        {
            bonus.Activated -= OnActivatedSacrificeBonus;
            
            _tankDispatcher.Unblock();
            
            UnblockAllBonuses();
        }
    }
}