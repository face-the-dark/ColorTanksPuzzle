using System;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Configurations.Levels;
using _Game.Code.Destroyers;
using _Game.Code.PersistenceProgress;
using _Game.Code.WaitingAreaComponents;
using VContainer;

namespace _Game.Code.Infrastructure
{
    public class GameOver : IDisposable
    {
        private readonly PixelDestroyer _pixelDestroyer;
        private readonly WaitingArea _waitingArea;
        private readonly IPlayerDataService _playerDataService;
        private readonly LevelConfigurationProvider _levelConfigurationProvider;

        public event Action GameWon;
        public event Action GameLost;

        [Inject]
        public GameOver
        (
            PixelDestroyer pixelDestroyer,
            WaitingArea waitingArea,
            IPlayerDataService playerDataService,
            LevelConfigurationProvider levelConfigurationProvider
        )
        {
            _pixelDestroyer = pixelDestroyer ?? throw new ArgumentNullException(nameof(pixelDestroyer));
            _waitingArea = waitingArea ?? throw new ArgumentNullException(nameof(waitingArea));
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));
            _levelConfigurationProvider = levelConfigurationProvider 
                                          ?? throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _pixelDestroyer.AllPixelsDestroyed += Win;
            _waitingArea.Overflowed += Lose;
        }

        public void Dispose()
        {
            _pixelDestroyer.AllPixelsDestroyed -= Win;
            _waitingArea.Overflowed -= Lose;
        }

        private void Win()
        {
            LevelConfiguration levelConfiguration = _levelConfigurationProvider.LevelConfiguration;
            int levelId = levelConfiguration.Id;
            DifficultyMode difficulty = levelConfiguration.DifficultyMode;
            _playerDataService.IncreaseMoney(levelConfiguration.RewardCoins);
            
            _playerDataService.MarkLevelPassed(levelId, difficulty);

            GameWon?.Invoke();
        }

        private void Lose()
        {
            _playerDataService.Reset();
            GameLost?.Invoke();
        }
    }
}