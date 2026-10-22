using System;
using _Game.Code.Configurations.Levels;
using _Game.Code.Infrastructure.SceneManagment;
using _Game.Code.Providers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class DifficultyButton : MonoBehaviour
    {
        private Button _button;
        private LevelConfiguration _levelConfiguration;

        private SceneLoader _sceneLoader;
        private LevelConfigurationProvider _levelConfigurationProvider;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, LevelConfigurationProvider levelConfigurationProvider)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _levelConfigurationProvider = levelConfigurationProvider 
                                          ?? throw new ArgumentNullException(nameof(levelConfigurationProvider));
        }

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnClicked);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClicked);

        public void SetLevelConfiguration(LevelConfiguration levelConfiguration)
        {
            _levelConfiguration = levelConfiguration;
        }

        private void OnClicked()
        {
            _levelConfigurationProvider.LevelConfiguration = _levelConfiguration;
            _sceneLoader.LoadGameScene();
        }
    }
}