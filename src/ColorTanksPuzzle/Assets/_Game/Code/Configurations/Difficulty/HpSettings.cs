using System;
using UnityEngine;

namespace _Game.Code.Configurations.Difficulty
{
    [Serializable]
    public class HpSettings
    {
        [SerializeField] private int _hpValue;
        [SerializeField] private int _hpWeight;
        [SerializeField] private int _hpDepth;
        
        public int HpValue => _hpValue;
        public int HpWeight => _hpWeight;
        public int HpDepth => _hpDepth;
    }
}