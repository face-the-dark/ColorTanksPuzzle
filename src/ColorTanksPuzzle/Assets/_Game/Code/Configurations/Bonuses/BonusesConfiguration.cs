using System.Collections.Generic;
using _Game.Code.Bonuses;
using UnityEngine;

namespace _Game.Code.Configurations.Bonuses
{
    [CreateAssetMenu(fileName =  "BonusesConfiguration", menuName = "Configurations/BonusesConfiguration")]
    public class BonusesConfiguration : ScriptableObject
    {
        [SerializeField] private float _freezeSplineBonusTime;
        [SerializeField] private List<BonusConfiguration> _bonusesConfigurations;
        
        public float FreezeSplineBonusTime => _freezeSplineBonusTime;

        public BonusConfiguration GetBonusConfiguration(Bonus bonus) => 
            _bonusesConfigurations.Find(x => x.Bonus == bonus);
    }
}