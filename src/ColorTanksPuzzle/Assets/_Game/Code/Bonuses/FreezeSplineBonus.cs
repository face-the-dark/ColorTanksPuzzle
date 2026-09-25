using System;
using _Game.Code.Players;
using _Game.Code.Providers;
using UnityEngine;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class FreezeSplineBonus : IBonus
    {
        private readonly TankDispatcher _tankDispatcher;
        private readonly BonusesConfigurationProvider _bonusesConfigurationProvider;

        private float _endTime;
        private bool _isBlocked;
        
        private bool IsActive => Time.time < _endTime;

        [Inject]
        public FreezeSplineBonus
        (
            TankDispatcher tankDispatcher,
            BonusesConfigurationProvider bonusesConfigurationProvider
        )
        {
            _tankDispatcher = tankDispatcher ?? throw new ArgumentNullException(nameof(tankDispatcher));

            _bonusesConfigurationProvider = bonusesConfigurationProvider ??
                                            throw new ArgumentNullException(nameof(bonusesConfigurationProvider));
        }

        public event Action<IBonus> Activated;
        
        public Bonus Bonus => Bonus.FreezeSpline;

        public void Activate()
        {
            if (IsActive == false)
            {
                _endTime = Time.time + _bonusesConfigurationProvider.BonusesConfiguration.FreezeSplineBonusTime;
                
                _tankDispatcher.FreezeTanksOnSpline();
            }
        }
        
        public void Block() => 
            _isBlocked = true;

        public void Unblock() => 
            _isBlocked = false;
    }
}