using System;
using _Game.Code.Bonuses;
using VContainer;

namespace _Game.Code.PersistenceProgress
{
    public class PlayerDataService
    {
        private readonly PlayerData _playerData;

        [Inject]
        public PlayerDataService(ISaveLoadService saveLoadService)
        {
            if (saveLoadService == null)
                throw new ArgumentNullException(nameof(saveLoadService));
            
            _playerData = saveLoadService.LoadPlayerData();
        }
        
        public event Action<int> MoneyChanged;
        public event Action<Bonus, int> BonusCountChanged;

        public void IncreaseMoney(int amount)
        {
            _playerData.Money += amount;
            
            MoneyChanged?.Invoke(_playerData.Money);
        }

        public void DeductMoney(int amount)
        {
            _playerData.Money -= amount;
            
            MoneyChanged?.Invoke(_playerData.Money);
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
    }
}