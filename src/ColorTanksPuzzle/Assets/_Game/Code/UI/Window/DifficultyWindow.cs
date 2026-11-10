using _Game.Code.Configurations.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI.Window
{
    public class DifficultyWindow : UserInterfaceWindow
    {
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private DifficultyButton _easyDifficultyButton;
        [SerializeField] private DifficultyButton _mediumDifficultyButton;
        [SerializeField] private DifficultyButton _hardDifficultyButton;
        [SerializeField] private Button _closeButton;

        private LevelConfiguration _easyLevelConfiguration;
        private LevelConfiguration _normalLevelConfiguration;
        private LevelConfiguration _hardLevelConfiguration;

        public void SetConfigurations
        (
            LevelConfiguration easyLevelConfiguration,
            LevelConfiguration normalLevelConfiguration,
            LevelConfiguration hardLevelConfiguration
        )
        {
            _easyLevelConfiguration = easyLevelConfiguration;
            _normalLevelConfiguration = normalLevelConfiguration;
            _hardLevelConfiguration = hardLevelConfiguration;

            Open();
        }

        private void Open()
        {
            _closeButton.onClick.AddListener(Close);
            
            InvokeOpen();

            _levelName.text = $"LEVEL {_easyLevelConfiguration.Id}";

            _easyDifficultyButton.SetLevelConfiguration(_easyLevelConfiguration);
            _mediumDifficultyButton.SetLevelConfiguration(_normalLevelConfiguration);
            _hardDifficultyButton.SetLevelConfiguration(_hardLevelConfiguration);
        }

        private void Close()
        {
            _closeButton.onClick.RemoveListener(Close);
            
            InvokeClose();
            
            _easyDifficultyButton.SetLevelConfiguration(null);
            _mediumDifficultyButton.SetLevelConfiguration(null);
            _hardDifficultyButton.SetLevelConfiguration(null);
        }
    }
}