using System;
using _Game.Code.Infrastructure;
using UnityEngine;
using VContainer;

namespace _Game.Code.UI.GameOverScreens
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private WinWindow _winWindow;
        [SerializeField] private LoseWindow _loseWindow;
        
        private GameOver _gameOver;
        
        [Inject]
        public void Construct(GameOver gameOver)
        {
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));
            
            _gameOver.GameWon += OnGameWon;
            _gameOver.GameLost += OnGameLost;
        }

        private void OnGameWon()
        {
            _gameOver.GameWon -= OnGameWon;
            _gameOver.GameLost -= OnGameLost;

            _winWindow.Open();
        }

        private void OnGameLost()
        {
            _gameOver.GameWon -= OnGameWon;
            _gameOver.GameLost -= OnGameLost;
            
            _loseWindow.Open();
        }
    }
}