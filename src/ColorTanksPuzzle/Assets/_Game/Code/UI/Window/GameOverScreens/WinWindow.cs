using System;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window.GameOverScreens
{
    public class WinWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _continueButton;

        private SceneLoader _sceneLoader;
        private GameOver _gameOver;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, GameOver gameOver)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));

            _gameOver.GameWon += Open;
        }

        protected override void OnOpen()
        {
            _gameOver.GameWon -= Open;
            
            _continueButton.onClick.AddListener(BackToMenu);
        }

        protected override void OnClose()
        {
        }

        private void BackToMenu()
        {
            Close();
            
            _continueButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadMenuScene();
        }
    }
}