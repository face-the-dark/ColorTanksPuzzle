using System;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Configurations.Levels;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.SceneManagment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window.GameOverScreens
{
    public class WinWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardCoins;

        private LevelConfigurationProvider _levelConfigurationProvider;
        private SceneLoader _sceneLoader;
        private GameOver _gameOver;

        [Inject]
        public void Construct(SceneLoader sceneLoader, GameOver gameOver,
            LevelConfigurationProvider levelConfigurationProvider)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _gameOver = gameOver ?? throw new ArgumentNullException(nameof(gameOver));
            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _gameOver.GameWon += Open;
        }

        private void Open()
        {
            _gameOver.GameWon -= Open;

            InvokeOpen();

            LevelConfiguration levelConfiguration = _levelConfigurationProvider.LevelConfiguration;
            int levelId = levelConfiguration.Id;
            _levelText.text = $"LEVEL {levelId}";
            _rewardImage.sprite = levelConfiguration.RewardSprite;
            string rewardCoins = levelConfiguration.RewardCoins.ToString();
            _rewardCoins.text = $"x{rewardCoins}";

            _continueButton.onClick.AddListener(BackToMenu);
        }

        private void BackToMenu()
        {
            InvokeClose();

            _continueButton.onClick.RemoveListener(BackToMenu);

            _sceneLoader.LoadMenuScene();
        }
    }
}