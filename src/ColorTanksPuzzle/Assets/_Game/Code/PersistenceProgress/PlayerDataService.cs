using System;
using _Game.Code.Bonuses;
using _Game.Code.Configurations.Difficulty;
using VContainer;

namespace _Game.Code.PersistenceProgress
{
    public class PlayerDataService
    {
        private readonly ISaveLoadService _saveLoadService;

        private PlayerData _currentPlayerData;
        private PlayerData _previousPlayerData;

        [Inject]
        public PlayerDataService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));

            _previousPlayerData = saveLoadService.LoadPlayerData();
            _currentPlayerData = _previousPlayerData.Clone();
        } 

        public void InitializeMoney() =>
            MoneyChanged?.Invoke(_currentPlayerData.Money);

        public void InitializeBonusesCounts()
        {
            foreach (Bonus bonus in Enum.GetValues(typeof(Bonus)))
                BonusCountChanged?.Invoke(bonus, GetBonusCount(bonus).Value);
        }

        public event Action<int> MoneyChanged;
        public event Action<Bonus, int> BonusCountChanged;

        public void IncreaseMoney(int amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException();

            _currentPlayerData.Money += amount;

            MoneyChanged?.Invoke(_currentPlayerData.Money);
        }

        public bool TryDeductMoney(int amount)
        {
            if (amount > 0 && amount <= _currentPlayerData.Money)
            {
                _currentPlayerData.Money -= amount;

                MoneyChanged?.Invoke(_currentPlayerData.Money);

                return true;
            }

            return false;
        }

        public void IncreaseBonusCount(Bonus bonus, int count)
        {
            BonusCount bonusCount = GetBonusCount(bonus);

            if (bonusCount == null)
                throw new InvalidOperationException();

            bonusCount.Value += count;

            BonusCountChanged?.Invoke(bonus, bonusCount.Value);
        }

        public void DecreaseBonusCount(Bonus bonus, int count)
        {
            BonusCount bonusCount = GetBonusCount(bonus);

            if (bonusCount == null)
                throw new InvalidOperationException();

            bonusCount.Value -= count;

            BonusCountChanged?.Invoke(bonus, bonusCount.Value);
        }

        public bool IsEnoughBonusCount(Bonus bonus)
        {
            BonusCount bonusCount = GetBonusCount(bonus);

            if (bonusCount == null)
                return false;

            return bonusCount.Value > 0;
        }

        public void MarkLevelPassed(int levelId, DifficultyMode difficulty)
        {
            PassedLevel level = _currentPlayerData.PassedLevels.Find(x => x.Id == levelId);

            if (level == null)
            {
                level = new PassedLevel
                {
                    Id = levelId
                };
                _currentPlayerData.PassedLevels.Add(level);
            }

            level.PassedDifficulties.Add(difficulty);

            SavePlayerData();
        }

        public bool IsLevelPassed(int levelId)
        {
            return _currentPlayerData.PassedLevels.Find
            (x =>
                x.Id == levelId
                && x.PassedDifficulties.Contains(DifficultyMode.Easy)
                && x.PassedDifficulties.Contains(DifficultyMode.Normal)
                && x.PassedDifficulties.Contains(DifficultyMode.Hard)
            ) != null;
        }

        public void Reset()
        {
            _currentPlayerData = _previousPlayerData.Clone();
            
            MoneyChanged?.Invoke(_currentPlayerData.Money);
            
            foreach (Bonus bonus in Enum.GetValues(typeof(Bonus)))
                BonusCountChanged?.Invoke(bonus, GetBonusCount(bonus).Value);
        }

        private BonusCount GetBonusCount(Bonus bonus) =>
            _currentPlayerData.BonusesCounts.Find(x => x.Bonus == bonus);

        private void SavePlayerData() =>
            _saveLoadService.SavePlayerData(_currentPlayerData);
    }
}