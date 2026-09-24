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

        private readonly Dictionary<Bonus, IBonus> _bonuses;

        [Inject]
        public BonusActivator(InputReader inputReader, IReadOnlyList<IBonus> bonuses)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));

            _bonuses = bonuses.ToDictionary(k => k.Bonus, bonus => bonus);

            _inputReader.ExpansionBonusUsed += ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed += ActivateFreezeSplineBonus;
        }

        public void Dispose()
        {
            _inputReader.ExpansionBonusUsed -= ActivateExpansionBonus;
            _inputReader.FreezeSplineBonusUsed -= ActivateFreezeSplineBonus;
        }

        public void ActivateBonus(Bonus bonus)
        {
            _bonuses.TryGetValue(bonus, out IBonus activeBonus);

            if (activeBonus == null)
                throw new InvalidOperationException(nameof(activeBonus));

            activeBonus.Activate();
        }

        private void ActivateExpansionBonus() => 
            ActivateBonus(Bonus.ExpansionBonus);
        
        private void ActivateFreezeSplineBonus() => 
            ActivateBonus(Bonus.FreezeSplineBonus);
    }
}