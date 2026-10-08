using System.Collections.Generic;
using _Game.Code.Bonuses;
using UnityEngine;

namespace _Game.Code.Configurations.Bonuses
{
    [CreateAssetMenu(fileName =  "BonusesConfiguration", menuName = "Configurations/BonusesConfiguration")]
    public class BonusesConfiguration : ScriptableObject
    {
        [SerializeField] private float _freezeSplineBonusTime;
        [SerializeField] private List<BonusPurchaseConfiguration> _bonusesPurchaseConfigurations;
        
        public float FreezeSplineBonusTime => _freezeSplineBonusTime;

        public BonusPurchaseConfiguration GetBonusPurchaseConfiguration(Bonus bonus) => 
            _bonusesPurchaseConfigurations.Find(x => x.Bonus == bonus);
    }
}