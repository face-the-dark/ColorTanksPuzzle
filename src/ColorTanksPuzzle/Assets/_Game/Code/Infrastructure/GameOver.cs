using System;
using _Game.Code.Destroyers;
using _Game.Code.PersistenceProgress;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using VContainer;

namespace _Game.Code.Infrastructure
{
    public class GameOver : IDisposable
    {
        private readonly PixelDestroyer _pixelDestroyer;
        private readonly WaitingArea _waitingArea;
        private readonly PlayerDataService _playerDataService;

        public event Action GameWon;
        public event Action GameLost;

        [Inject]
        public GameOver(PixelDestroyer pixelDestroyer, WaitingArea waitingArea, PlayerDataService playerDataService)
        {
            _pixelDestroyer = pixelDestroyer ?? throw new ArgumentNullException(nameof(pixelDestroyer));
            _waitingArea = waitingArea ?? throw new ArgumentNullException(nameof(waitingArea));
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));

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
            Debug.Log("Win");
            
            _playerDataService.SavePlayerData();
            
            GameWon?.Invoke();
        }

        private void Lose()
        {
            Debug.Log("Lose");
            GameLost?.Invoke();
        }
    }
}