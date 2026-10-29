using System;
using _Game.Code.Bonuses;
using UnityEngine;

namespace _Game.Code.Configurations.Bonuses
{
    [Serializable]
    public class BonusConfiguration
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Bonus _bonus;
        [SerializeField] private int _count;
        [SerializeField] private int _cost;
        
        public string Name => _name;
        public Sprite Icon => _icon;
        public Bonus Bonus => _bonus;
        public int Cost => _cost;
        public int Count => _count;
    }
}