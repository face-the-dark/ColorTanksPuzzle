using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code.Difficulty
{
    public class DifficultySettingsProvider : MonoBehaviour
    {
        [SerializeField] private List<DifficultySettings> _difficultySettings;
        
        private Dictionary<DifficultyMode, DifficultySettings> _settingsMap;

        private void Awake()
        {
            _settingsMap = new Dictionary<DifficultyMode, DifficultySettings>();
            
            foreach (DifficultySettings difficultySetting in _difficultySettings) 
                _settingsMap.Add(difficultySetting.DifficultyMode, difficultySetting);
        }

        public DifficultySettings Get(DifficultyMode difficultyMode) => 
            _settingsMap.GetValueOrDefault(difficultyMode);
    }
}