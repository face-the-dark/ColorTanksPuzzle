using System;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.GameOverScreens
{
    [RequireComponent(typeof(Canvas))]
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        
        private Canvas _canvas;

        private SceneLoader _sceneLoader;
        private GameOver _gameOver;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, GameOver gameOver)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));

            _gameOver.GameWon += Open;
        }
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _canvas.enabled = false;
        }

        private void Open()
        {
            _gameOver.GameWon -= Open;
            
            _canvas.enabled = true;
            
            _continueButton.onClick.AddListener(BackToMenu);
        }
        
        private void BackToMenu()
        {
            _canvas.enabled = false;
            
            _continueButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadMenuScene();
        }
    }
}