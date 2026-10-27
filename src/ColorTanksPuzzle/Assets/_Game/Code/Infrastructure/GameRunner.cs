using _Game.Code.Configurations.Levels;
using _Game.Code.Providers;
using UnityEngine;
using VContainer;

namespace _Game.Code.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private LevelConfiguration _defaultLevelConfiguration;

        private LevelConfigurationProvider _levelConfigurationProvider;

        [Inject]
        public void Construct(LevelConfigurationProvider levelConfigurationProvider)
        {
            _levelConfigurationProvider = levelConfigurationProvider;
        }

        public void Initialize()
        {
            if (_levelConfigurationProvider.LevelConfiguration == null)
                _levelConfigurationProvider.LevelConfiguration = _defaultLevelConfiguration;
        }
    }
}