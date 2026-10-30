using System;
using _Game.Code.Configurations.Levels;
using _Game.Code.PersistenceProgress;
using _Game.Code.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private DifficultyWindow _difficultyWindow;
        [SerializeField] private LevelConfiguration _easyLevelConfiguration;
        [SerializeField] private LevelConfiguration _normalLevelConfiguration;
        [SerializeField] private LevelConfiguration _hardLevelConfiguration;

        private Button _button;

        private PlayerDataService _playerDataService;
        
        [Inject]
        public void Construct(PlayerDataService playerDataService)
        {
            _playerDataService = playerDataService ?? throw new ArgumentNullException(nameof(playerDataService));
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (_playerDataService.IsLevelPassed(_easyLevelConfiguration.Id))
            {
                _button.interactable = false;
            }
            else
            {
                _button.onClick.AddListener(ChooseDifficulty);
            }
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ChooseDifficulty);
        }

        private void ChooseDifficulty()
        {
            _difficultyWindow.SetConfigurations
            (
                 _easyLevelConfiguration,
                 _normalLevelConfiguration,
                 _hardLevelConfiguration
            );
        }
    }
}