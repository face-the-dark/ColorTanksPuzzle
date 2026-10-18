using _Game.Code.Configurations.Levels;
using UnityEngine;
using UnityEngine.UI;

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

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ChooseDifficulty);
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