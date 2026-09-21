using System.Collections.Generic;
using System.Linq;
using _Game.Code.Configurations.Difficulty;
using UnityEngine;

namespace _Game.Code.Difficulty
{
    [CreateAssetMenu(fileName = "DifficultySettings", menuName = "Configurations/DifficultySettings")]
    public class DifficultySettings : ScriptableObject
    {
        [SerializeField] private DifficultyMode _difficultyMode;
        [SerializeField] private List<HpSettings> _hpSettings;

        public DifficultyMode DifficultyMode => _difficultyMode;
        public IReadOnlyList<HpSettings> HpSettings => _hpSettings;

        public int[] HpValues => _hpSettings.Select(x => x.HpValue).ToArray();
        public int[] HpWeights => _hpSettings.Select(x => x.HpWeight).ToArray();
        public int[] HpDepths => _hpSettings.Select(x => x.HpDepth).ToArray();
    }
}