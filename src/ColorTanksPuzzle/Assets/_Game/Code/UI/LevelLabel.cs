using _Game.Code.Configurations.Difficulty;
using _Game.Code.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    public class LevelLabel : MonoBehaviour
    {
        [SerializeField] private Color _colorForEasyDifficulty;
        [SerializeField] private Color _colorForNormalDifficulty;
        [SerializeField] private Color _colorForHardDifficulty;

        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _text;

        private LevelConfigurationProvider _levelConfigurationProvider;
        
        [Inject]
        public void Construct(LevelConfigurationProvider levelConfigurationProvider)
        {
            _levelConfigurationProvider = levelConfigurationProvider;
        }

        private void Start()
        {
            int levelId = _levelConfigurationProvider.LevelConfiguration.Id;

            _background.color = ChooseColorByDifficulty();
            _text.text = $"LEVEL {levelId}";
        }

        private Color ChooseColorByDifficulty()
        {
            DifficultyMode difficultyMode = _levelConfigurationProvider.LevelConfiguration.DifficultyMode;
            
            switch (difficultyMode)
            {
                case DifficultyMode.Easy:
                    return _colorForEasyDifficulty;
                
                case DifficultyMode.Normal:
                    return _colorForNormalDifficulty;
                
                case DifficultyMode.Hard:
                    return _colorForHardDifficulty;
            }
            
            return Color.white;
        }
    }
}