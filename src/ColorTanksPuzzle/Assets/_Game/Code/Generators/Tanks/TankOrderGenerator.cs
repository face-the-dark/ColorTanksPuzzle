using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Data;
using _Game.Code.Providers;
using _Game.Code.Utilities;

namespace _Game.Code.Generators.Tanks
{
    public class TankOrderGenerator
    {
        private readonly DifficultyConfigurationProvider _difficultyConfigurationProvider;
        private readonly LevelConfigurationProvider _levelConfigurationProvider;
        private readonly DeterministicRandom _random;

        public TankOrderGenerator
        (
            DifficultyConfigurationProvider difficultyConfigurationProvider,
            LevelConfigurationProvider levelConfigurationProvider,
            DeterministicRandom random
        )
        {
            _difficultyConfigurationProvider = difficultyConfigurationProvider ??
                                               throw new ArgumentNullException(nameof(difficultyConfigurationProvider));

            _levelConfigurationProvider = levelConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(levelConfigurationProvider));
            
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public List<TankData> OrderByDepth(List<TankData> tanks)
        {
            if (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            List<TankData> remainingTanks = new List<TankData>(tanks);
            List<TankData> orderedTanks = new();

            while (remainingTanks.Count > 0)
            {
                int depth = SelectDepth(remainingTanks);

                TankData tank = TakeRandomTankByDepth(remainingTanks, depth);

                orderedTanks.Add(tank);
            }

            return orderedTanks;
        }

        private int SelectDepth(List<TankData> tanks)
        {
            if (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            List<int> possibleDepths = new();
            List<int> depthWeights = new();

            DifficultyMode difficultyMode = _levelConfigurationProvider.LevelConfiguration.DifficultyMode;

            List<DepthConfiguration> depthConfigurations =
                _difficultyConfigurationProvider.Get(difficultyMode).DepthConfigurations;

            if (depthConfigurations == null || depthConfigurations.Count == 0)
                throw new ArgumentNullException(nameof(depthConfigurations));

            foreach (DepthConfiguration depthConfiguration in depthConfigurations)
            {
                AddDepthOption
                (
                    tanks,
                    depthConfiguration.DepthValue,
                    depthConfiguration.DepthWeight,
                    possibleDepths,
                    depthWeights
                );
            }

            return SelectWeightedValue(possibleDepths, depthWeights);
        }

        private void AddDepthOption
        (
            List<TankData> tanks,
            int depth,
            int weight,
            List<int> possibleDepths,
            List<int> depthWeights
        )
        {
            if (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            if (possibleDepths == null)
                throw new ArgumentNullException(nameof(possibleDepths));

            if (depthWeights == null)
                throw new ArgumentNullException(nameof(depthWeights));

            if (weight <= 0)
                return;

            bool isDepthExists = tanks.Any(tank => tank.Depth == depth);

            if (isDepthExists)
            {
                possibleDepths.Add(depth);
                depthWeights.Add(weight);
            }
        }

        private int SelectWeightedValue(List<int> values, List<int> weights)
        {
            if (values == null || values.Count == 0)
                throw new ArgumentNullException(nameof(values));

            if (weights == null || weights.Count == 0)
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

        private TankData TakeRandomTankByDepth(List<TankData> tanks, int depth)
        {
            if (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            List<int> matchingIndexes = new();

            for (int i = 0; i < tanks.Count; i++)
                if (tanks[i].Depth == depth)
                    matchingIndexes.Add(i);

            int randomIndex = _random.NextInt(0, matchingIndexes.Count);

            int tankIndex = matchingIndexes[randomIndex];

            TankData selectedTank = tanks[tankIndex];

            tanks.RemoveAt(tankIndex);

            return selectedTank;
        }
    }
}