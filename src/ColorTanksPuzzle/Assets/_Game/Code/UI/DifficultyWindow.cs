using _Game.Code.Configurations.Levels;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    public class DifficultyWindow : UserInterfaceWindow
    {
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private DifficultyButton _easyDifficultyButton;
        [SerializeField] private DifficultyButton _mediumDifficultyButton;
        [SerializeField] private DifficultyButton _hardDifficultyButton;

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
            _easyLevelConfiguration =  easyLevelConfiguration;
            _normalLevelConfiguration = normalLevelConfiguration;
            _hardLevelConfiguration = hardLevelConfiguration;
            
            Open();
        }

        protected override void OnOpen()
        {
            _levelName.text = $"LEVEL {_easyLevelConfiguration.Id}";
            
            _easyDifficultyButton.SetLevelConfiguration(_easyLevelConfiguration);
            _mediumDifficultyButton.SetLevelConfiguration(_normalLevelConfiguration);
            _hardDifficultyButton.SetLevelConfiguration(_hardLevelConfiguration);
        }

        protected override void OnClose()
        {
            SetNullableConfigurations();
        }

        private void SetNullableConfigurations()
        {
            _easyDifficultyButton.SetLevelConfiguration(null);
            _mediumDifficultyButton.SetLevelConfiguration(null);
            _hardDifficultyButton.SetLevelConfiguration(null);
        }
    }
}