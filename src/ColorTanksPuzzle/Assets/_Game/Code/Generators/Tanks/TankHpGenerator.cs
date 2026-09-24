using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Providers;
using _Game.Code.Utilities;
using VContainer;

namespace _Game.Code.Generators.Tanks
{
    public class TankHpGenerator
    {
        private const int MinimumStandardHp = 10;

        private readonly LevelConfigurationProvider _levelConfigurationProvider;
        private readonly DeterministicRandom _random;

        [Inject]
        public TankHpGenerator(LevelConfigurationProvider levelConfigurationProvider, DeterministicRandom random)
        {
            _levelConfigurationProvider = levelConfigurationProvider
                                          ?? throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public List<int> GenerateTanksHpByPixelsCount(int pixelCount)
        {
            if (pixelCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pixelCount));

            List<int> hpValues = new();

            int remainingPixels = pixelCount;

            while (remainingPixels >= MinimumStandardHp)
            {
                int hp = SelectHp(remainingPixels);

                hpValues.Add(hp);

                remainingPixels -= hp;
            }

            if (remainingPixels > 0)
            {
                hpValues.Add(remainingPixels);
            }

            return hpValues;
        }

        private int SelectHp(int remainingPixels)
        {
            if (remainingPixels <= 0)
                throw new ArgumentOutOfRangeException(nameof(remainingPixels));

            List<int> possibleHp = new();
            List<int> weights = new();
            
            List<HpConfiguration> hpConfigurations = 
                _levelConfigurationProvider.GetDifficultyConfiguration().HpConfigurations;

            if (hpConfigurations == null || hpConfigurations.Count == 0)
                throw new ArgumentNullException(nameof(hpConfigurations));

            foreach (HpConfiguration hpConfiguration in hpConfigurations)
                AddHpOption(hpConfiguration.HpValue, hpConfiguration.HpWeight, remainingPixels, possibleHp, weights);

            return SelectWeightedValue(possibleHp, weights);
        }

        private void AddHpOption
        (
            int hp,
            int weight,
            int remainingPixels,
            List<int> possibleHp,
            List<int> weights
        )
        {
            if (remainingPixels <= 0)
                throw new ArgumentOutOfRangeException(nameof(remainingPixels));
            
            if (possibleHp == null)
                throw new ArgumentNullException(nameof(possibleHp));
            
            if  (weights == null)
                throw new ArgumentNullException(nameof(weights));
            
            if (hp > remainingPixels)
                return;

            possibleHp.Add(hp);
            weights.Add(weight);
        }

        private int SelectWeightedValue(List<int> values, List<int> weights)
        {
            if (values == null || values.Count == 0)
                throw new ArgumentNullException(nameof(values));
            
            if  (weights == null || weights.Count == 0)
                throw new ArgumentNullException(nameof(weights));
            
            int totalWeight = weights.Sum();

            int randomValue = _random.NextInt(0, totalWeight);

            int accumulatedWeight = 0;

            for (int i = 0; i < values.Count; i++)
            {
                accumulatedWeight += weights[i];

                if (randomValue < accumulatedWeight)
                    return values[i];
            }

            return values[^1];
        }
    }
}