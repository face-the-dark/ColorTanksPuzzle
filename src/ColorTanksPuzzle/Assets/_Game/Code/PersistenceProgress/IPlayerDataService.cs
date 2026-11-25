using System;
using _Game.Code.Bonuses;
using _Game.Code.Configurations.Difficulty;

namespace _Game.Code.PersistenceProgress
{
    public interface IPlayerDataService
    {
        event Action<int> MoneyChanged;
        event Action<Bonus, int> BonusCountChanged;
        
        void Initialize();
        void IncreaseMoney(int amount);
        bool TryDeductMoney(int amount);
        bool IsEnoughBonusCount(Bonus bonus);
        void IncreaseBonusCount(Bonus bonus, int count);
        void DecreaseBonusCount(Bonus bonus, int count);
        bool IsLevelPassed(int levelId);
        bool IsDifficultyPassed(int levelId, DifficultyMode difficultyMode);
        void MarkLevelPassed(int levelId, DifficultyMode difficulty);
        void Reset();
    }
}