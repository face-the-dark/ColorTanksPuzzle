using UnityEngine;

namespace _Game.Code.Configurations.Bonuses
{
    [CreateAssetMenu(fileName =  "BonusesConfiguration", menuName = "Configurations/BonusesConfiguration")]
    public class BonusesConfiguration : ScriptableObject
    {
        [SerializeField] private float _freezeSplineBonusTime;
        
        public float FreezeSplineBonusTime => _freezeSplineBonusTime;
    }
}