using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code.Configurations.Difficulty
{
    [CreateAssetMenu(fileName = "DifficultyConfiguration", menuName = "Configurations/Difficulty Configuration")]
    public class DifficultyConfiguration : ScriptableObject
    {
        [SerializeField] private DifficultyMode _difficultyMode;
        [SerializeField] private List<HpConfiguration> _hpConfigurations;
        [SerializeField] private List<DepthConfiguration> _depthConfigurations;
        [SerializeField] private int _startMaxTanksCount;

        public DifficultyMode DifficultyMode => _difficultyMode;
        public List<HpConfiguration> HpConfigurations => _hpConfigurations;
        public List<DepthConfiguration> DepthConfigurations => _depthConfigurations;
        public int StartMaxTanksCount => _startMaxTanksCount;
    }
}