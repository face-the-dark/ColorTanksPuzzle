using System;
using _Game.Code.Providers;
using VContainer;

namespace _Game.Code.Utilities
{
    public class DeterministicRandom
    {
        private readonly LevelConfigurationProvider _levelConfigurationProvider;
        
        private Random _random;

        [Inject]
        public DeterministicRandom(LevelConfigurationProvider levelConfigurationProvider)
        {
            _levelConfigurationProvider = levelConfigurationProvider 
                                          ?? throw new ArgumentNullException(nameof(levelConfigurationProvider));
        }

        public void Initialize() => 
            _random = new Random(_levelConfigurationProvider.LevelConfiguration.Seed);

        public int NextInt(int minInclusive, int maxExclusive) => 
            _random.Next(minInclusive, maxExclusive);
    }
}