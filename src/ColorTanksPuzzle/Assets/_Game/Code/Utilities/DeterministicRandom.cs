using System;
using _Game.Code.Providers;

namespace _Game.Code.Utilities
{
    public class DeterministicRandom
    {
        private readonly Random _random;

        public DeterministicRandom(LevelConfigurationProvider levelConfigurationProvider) => 
            _random = new Random(levelConfigurationProvider.LevelConfiguration.Seed);

        public int NextInt(int minInclusive, int maxExclusive) => 
            _random.Next(minInclusive, maxExclusive);
    }
}