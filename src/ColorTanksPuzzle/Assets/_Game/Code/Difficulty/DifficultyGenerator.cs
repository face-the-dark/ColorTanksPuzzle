using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Data;
using UnityEngine;

namespace _Game.Code.Difficulty
{
    public class DifficultyGenerator : MonoBehaviour
    {
        [SerializeField] private DifficultySettingsProvider _difficultySettingsProvider;

        public List<TankData> Generate
        (
            List<TankData> source,
            DifficultyMode difficultyMode,
            int laneCount,
            int seed
        )
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (laneCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(laneCount));

            if (source.Count == 0)
                return new List<TankData>();

            DifficultySettings settings = _difficultySettingsProvider.Get(difficultyMode);
            DeterministicRandom random = new DeterministicRandom(seed);

            List<TankData> result = new List<TankData>(source);

            ShuffleWithinDepth(result, random);
            result = BuildDepthOrderedList(result, settings, random);
            result = ApplyLocalShuffle(result, difficultyMode, random);
            AssignLanes(result, laneCount);

            return result;
        }

        private static void ShuffleWithinDepth(List<TankData> tanks, DeterministicRandom random)
        {
            for (int depth = 0; depth <= 2; depth++)
            {
                List<int> indexes = new();

                for (int i = 0; i < tanks.Count; i++)
                {
                    if (tanks[i].Depth == depth)
                        indexes.Add(i);
                }

                for (int i = indexes.Count - 1; i > 0; i--)
                {
                    int j = random.NextInt(0, i + 1);

                    (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
                }

                List<TankData> depthTanks = new();

                foreach (int index in indexes)
                    depthTanks.Add(tanks[index]);

                for (int i = 0; i < indexes.Count; i++)
                    tanks[indexes[i]] = depthTanks[i];
            }
        }

        private static List<TankData> BuildDepthOrderedList
        (
            List<TankData> tanks,
            DifficultySettings settings,
            DeterministicRandom random
        )
        {
            List<TankData> remaining = new List<TankData>(tanks);
            List<TankData> result = new List<TankData>(tanks.Count);

            while (remaining.Count > 0)
            {
                int depth = SelectDepth(remaining, settings.HpDepths, random);

                List<int> candidates = new();

                for (int i = 0; i < remaining.Count; i++)
                {
                    if (remaining[i].Depth == depth)
                        candidates.Add(i);
                }

                if (candidates.Count == 0)
                {
                    int index = random.NextInt(0, remaining.Count);

                    result.Add(remaining[index]);
                    remaining.RemoveAt(index);

                    continue;
                }

                int selectedCandidate = candidates[random.NextInt(0, candidates.Count)];

                result.Add(remaining[selectedCandidate]);
                remaining.RemoveAt(selectedCandidate);
            }

            return result;
        }

        private static int SelectDepth(List<TankData> tanks, int[] weights, DeterministicRandom random)
        {
            int totalWeight = 0;

            for (int depth = 0; depth < weights.Length; depth++)
            {
                bool exists = tanks.Any(x => x.Depth == depth);

                if (exists)
                    totalWeight += weights[depth];
            }

            if (totalWeight <= 0)
                return tanks[0].Depth;

            int roll = random.NextInt(0, totalWeight);

            int accumulated = 0;

            for (int depth = 0; depth < weights.Length; depth++)
            {
                bool exists = tanks.Any(x => x.Depth == depth);

                if (!exists)
                    continue;

                accumulated += weights[depth];

                if (roll < accumulated)
                    return depth;
            }

            return tanks[0].Depth;
        }

        private List<TankData> ApplyLocalShuffle
        (
            List<TankData> tanks,
            DifficultyMode difficultyMode,
            DeterministicRandom random
        )
        {
            int windowSize = difficultyMode switch
            {
                DifficultyMode.Easy => 4,
                DifficultyMode.Normal => 3,
                DifficultyMode.Hard => 2,
                _ => 3
            };

            List<TankData> result = new List<TankData>(tanks);

            for (int i = 0; i < result.Count; i++)
            {
                int maxIndex = Math.Min(i + windowSize, result.Count - 1);

                if (maxIndex <= i)
                    continue;

                int swapIndex = random.NextInt(i, maxIndex + 1);

                (result[i], result[swapIndex]) = (result[swapIndex], result[i]);
            }

            return result;
        }

        private void AssignLanes(List<TankData> tanks, int laneCount)
        {
            for (int i = 0; i < tanks.Count; i++)
            {
                tanks[i] = new TankData(
                    tanks[i].Color,
                    tanks[i].Hp,
                    i % laneCount,
                    tanks[i].Depth
                );
            }
        }
    }
}