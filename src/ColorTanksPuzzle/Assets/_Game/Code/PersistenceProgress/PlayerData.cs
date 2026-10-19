using System;
using System.Collections.Generic;
using _Game.Code.Bonuses;
using _Game.Code.Configurations.Difficulty;

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
            
            PassedLevels = new List<PassedLevel>();
        }

        public PlayerData(int money, List<BonusCount> bonusesCounts)
        {
            Money = money;
            BonusesCounts = bonusesCounts;
        }

        public int Money;
        public List<BonusCount> BonusesCounts = new();
        public List<PassedLevel> PassedLevels = new();
    }

    [Serializable]
    public class PassedLevel
    {
        public int Id;
        public List<DifficultyMode> PassedDifficulties = new();
    }
}