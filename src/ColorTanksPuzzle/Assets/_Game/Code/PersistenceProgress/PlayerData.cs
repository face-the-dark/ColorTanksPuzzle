using System;
using System.Collections.Generic;
using _Game.Code.Bonuses;

namespace _Game.Code.PersistenceProgress
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData()
        {
            Money = 0;
            
            BonusesCounts = new List<BonusCount> 
            {
                new(Bonus.Expansion, 0),
                new(Bonus.FreezeSpline, 0),
                new(Bonus.Sacrifice, 0),
                new(Bonus.ColorRocket, 0)
            };
        }

        public PlayerData(int money, List<BonusCount> bonusesCounts)
        {
            Money = money;
            BonusesCounts = bonusesCounts;
        }

        public int Money { get; set; }
        public List<BonusCount> BonusesCounts { get; set; }
    }
}