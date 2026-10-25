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
        [SerializeField] private Sprite _spriteForEasyDifficulty;
        [SerializeField] private Sprite _spriteForNormalDifficulty;
        [SerializeField] private Sprite _spriteForHardDifficulty;

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

            _background.sprite = ChooseColorByDifficulty();
            _text.text = $"LEVEL {levelId}";
        }

        private Sprite ChooseColorByDifficulty()
        {
            DifficultyMode difficultyMode = _levelConfigurationProvider.LevelConfiguration.DifficultyMode;
            
            switch (difficultyMode)
            {
                case DifficultyMode.Easy:
                    return _spriteForEasyDifficulty;
                
                case DifficultyMode.Normal:
                    return _spriteForNormalDifficulty;
                
                case DifficultyMode.Hard:
                    return _spriteForHardDifficulty;
                
                default:
                    return null;
            }
        }
    }
}