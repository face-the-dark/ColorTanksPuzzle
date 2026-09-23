using _Game.Code.Configurations.Levels;
using _Game.Code.Infrastructure;
using _Game.Code.Providers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelConfiguration _levelConfiguration;

        private Button _button;
        
        private SceneLoader _sceneLoader;
        
        [Inject]
        public void Construct(SceneLoader sceneLoader, LevelConfigurationProvider levelConfigurationProvider)
        {
            _sceneLoader = sceneLoader;

            levelConfigurationProvider.LevelConfiguration = _levelConfiguration;
        }

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnClicked);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClicked);

        private void OnClicked() => 
            _sceneLoader.LoadGameScene();
    }
}