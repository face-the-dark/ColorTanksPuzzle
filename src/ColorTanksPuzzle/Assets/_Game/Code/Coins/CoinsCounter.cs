using System;
using _Game.Code.PersistenceProgress;
using VContainer;

namespace _Game.Code.Coins
{
    public class CoinsCounter
    {
        private readonly PlayerDataService _playerDataService;

        [Inject]
        public CoinsCounter(PlayerDataService playerDataService)
        {
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));
        }

        public void IncreaseMoney()
        {
            _playerDataService.IncreaseMoney(1);
        }
    }
}