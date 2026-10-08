using System;
using _Game.Code.Bonuses;
using UnityEngine;

namespace _Game.Code.Configurations.Bonuses
{
    [Serializable]
    public class BonusPurchaseConfiguration
    {
        [SerializeField] private Bonus _bonus;
        [SerializeField] private int _count;
        [SerializeField] private int _cost;
        
        public Bonus Bonus => _bonus;
        public int Cost => _cost;
        public int Count => _count;
    }
}