using System;
using _Game.Code.PersistenceProgress;
using VContainer;

namespace _Game.Code.Coins
{
    public class CoinsCounter
    {
        private const int Amount = 1;
        
        private readonly IPlayerDataService _playerDataService;

        [Inject]
        public CoinsCounter(IPlayerDataService playerDataService) => 
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));

        public void IncreaseMoney() => 
            _playerDataService.IncreaseMoney(Amount);
    }
}