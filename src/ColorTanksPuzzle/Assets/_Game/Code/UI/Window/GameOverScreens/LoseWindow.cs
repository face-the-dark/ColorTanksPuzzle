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

        protected override void OnOpen()
        {
            _gameOver.GameLost -= Open;
            
            _tryAgainButton.onClick.AddListener(ReloadGame);
            CloseButton.onClick.AddListener(BackToMenu);
        }

        protected override void OnClose()
        {
        }

        private void ReloadGame()
        {
            Close();

            _tryAgainButton.onClick.RemoveListener(ReloadGame);
            CloseButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadGameScene();
        }

        private void BackToMenu()
        {
            Close();

            _tryAgainButton.onClick.RemoveListener(ReloadGame);
            CloseButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadMenuScene();
        }
    }
}