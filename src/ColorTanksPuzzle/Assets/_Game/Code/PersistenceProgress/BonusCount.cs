using System;
using _Game.Code.Bonuses;

namespace _Game.Code.PersistenceProgress
{
    [Serializable]
    public class BonusCount
    {
        public BonusCount(Bonus bonus, int value)
        {
            Bonus = bonus;
            Value = value;
        }

        public Bonus Bonus { get; set; }
        public int Value { get; set; }
    }
}