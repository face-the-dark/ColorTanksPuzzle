using System;
using _Game.Code.Bonuses;
using VContainer;

namespace _Game.Code.PersistenceProgress
{
    public class PlayerDataService
    {
        private readonly ISaveLoadService _saveLoadService;
        
        private readonly PlayerData _playerData;

        [Inject]
        public PlayerDataService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));

            _playerData = saveLoadService.LoadPlayerData();
        }

        public void InitializeMoney() => 
            MoneyChanged?.Invoke(_playerData.Money);

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
            
            _playerData.Money += amount;
            
            MoneyChanged?.Invoke(_playerData.Money);
        }

        public void DeductMoney(int amount)
        {
            if (amount > 0 && amount <= _playerData.Money)
            {
                _playerData.Money -= amount;
            
                MoneyChanged?.Invoke(_playerData.Money);
            }
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

        private BonusCount GetBonusCount(Bonus bonus) =>
            _playerData.BonusesCounts.Find(x => x.Bonus == bonus);

        public void SavePlayerData() => 
            _saveLoadService.SavePlayerData(_playerData);
    }
}