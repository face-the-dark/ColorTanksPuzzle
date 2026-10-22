using System;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.GameOverScreens
{
    [RequireComponent(typeof(Canvas))]
    public class LoseWindow : MonoBehaviour
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _closeButton;
        
        private Canvas _canvas;

        private SceneLoader _sceneLoader;
        private GameOver _gameOver;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, GameOver gameOver)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));
            
            _gameOver.GameLost += Open;
        }
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _canvas.enabled = false;
        }

        private void Open()
        {
            _gameOver.GameLost -= Open;
            
            _canvas.enabled = true;
            
            _tryAgainButton.onClick.AddListener(ReloadGame);
            _closeButton.onClick.AddListener(BackToMenu);
        }

        private void ReloadGame()
        {
            _canvas.enabled = false;
            
            _tryAgainButton.onClick.RemoveListener(ReloadGame);
            _closeButton.onClick.RemoveListener(BackToMenu);
            
            _sceneLoader.LoadGameScene();
        }

        private void BackToMenu()
        {
            _canvas.enabled = false;
            
            _tryAgainButton.onClick.RemoveListener(ReloadGame);
            _closeButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadMenuScene();
        }
    }
}