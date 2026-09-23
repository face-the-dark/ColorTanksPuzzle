using System;
using UnityEngine;

namespace _Game.Code.Configurations.Difficulty
{
    [Serializable]
    public class HpConfiguration
    {
        [SerializeField] private int _hpValue;
        [SerializeField] private int _hpWeight;
        
        public int HpValue => _hpValue;
        public int HpWeight => _hpWeight;
    }
}