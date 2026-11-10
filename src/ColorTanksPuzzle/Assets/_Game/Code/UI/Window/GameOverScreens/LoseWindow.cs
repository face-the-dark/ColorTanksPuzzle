using System;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window.GameOverScreens
{
    public class LoseWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _tryAgainButton;

        private SceneLoader _sceneLoader;
        private GameOver _gameOver;

        [Inject]
        public void Construct(SceneLoader sceneLoader, GameOver gameOver)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));

            _gameOver.GameLost += Open;
        }

        private void Open()
        {
            _gameOver.GameLost -= Open;
            
            InvokeOpen();
            
            _menuButton.onClick.AddListener(BackToMenu);
            _tryAgainButton.onClick.AddListener(ReloadGame);
        }

        private void ReloadGame()
        {
            InvokeClose();
            
            _menuButton.onClick.RemoveListener(BackToMenu);
            _tryAgainButton.onClick.RemoveListener(ReloadGame);

            _sceneLoader.LoadGameScene();
        }

        private void BackToMenu()
        {
            InvokeClose();
            
            _menuButton.onClick.RemoveListener(BackToMenu);
            _tryAgainButton.onClick.RemoveListener(ReloadGame);

            _sceneLoader.LoadMenuScene();
        }
    }
}