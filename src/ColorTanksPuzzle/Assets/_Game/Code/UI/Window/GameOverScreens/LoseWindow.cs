using System;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window.GameOverScreens
{
    public class LoseWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private TextMeshProUGUI _levelText;

        private LevelConfigurationProvider _levelConfigurationProvider;
        private ISceneLoader _sceneLoader;
        private GameOver _gameOver;

        [Inject]
        public void Construct
        (
            ISceneLoader sceneLoader,
            GameOver gameOver,
            LevelConfigurationProvider levelConfigurationProvider
        )
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));
            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _gameOver.GameLost += Open;
        }

        private void Start()
        {
            int levelId = _levelConfigurationProvider.LevelConfiguration.Id;
            _levelText.text = $"LEVEL {levelId}";
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