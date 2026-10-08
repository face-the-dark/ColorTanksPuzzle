using System;
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
        private readonly ISaveLoadService _saveLoadService;

        public event Action GameWon;
        public event Action GameLost;

        [Inject]
        public GameOver(PixelDestroyer pixelDestroyer, WaitingArea waitingArea, ISaveLoadService saveLoadService)
        {
            _pixelDestroyer = pixelDestroyer ?? throw new ArgumentNullException(nameof(pixelDestroyer));
            _waitingArea = waitingArea ?? throw new ArgumentNullException(nameof(waitingArea));
            _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));

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
            GameWon?.Invoke();
        }

        private void Lose()
        {
            GameLost?.Invoke();
        }
    }
}