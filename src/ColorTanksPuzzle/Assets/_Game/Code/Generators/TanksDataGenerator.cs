using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Data;
using _Game.Code.Difficulty;
using UnityEngine;

namespace _Game.Code.Generators
{
    public class TanksDataGenerator : MonoBehaviour
    {
        private const int LanesCount = 3;
        private const int MinResidualHp = 5;

        [SerializeField] private int _seed = 12345;
        [SerializeField] private DifficultyMode _difficultyMode = DifficultyMode.Easy;
        
        [SerializeField] private PixelArtGenerator _pixelArtGenerator;
        [SerializeField] private DifficultyGenerator _difficultyGenerator;
        [SerializeField] private DifficultySettingsProvider _difficultySettingsProvider;

        public event Action<List<TankData>> DataGenerated;

        private void OnEnable() =>
            _pixelArtGenerator.ArtGenerated += GenerateTanksData;

        private void OnDisable() =>
            _pixelArtGenerator.ArtGenerated -= GenerateTanksData;

        private void GenerateTanksData(List<PixelData> pixelsData)
        {
            List<TankData> tanksData = new();

            Dictionary<Color, List<PixelData>> pixelsByColor = GroupPixelsByColor(pixelsData);

            int colorIndex = 0;

            IOrderedEnumerable<KeyValuePair<Color, List<PixelData>>> keyValuePairs = pixelsByColor
                .OrderBy(x => x.Key.GetHashCode());

            foreach (KeyValuePair<Color, List<PixelData>> pair in keyValuePairs)
            {
                Color color = pair.Key;

                List<PixelData> pixels = pair.Value
                    .OrderBy(x => x.Depth)
                    .ToList();

                int seed = _seed + colorIndex * 7919;

                List<int> hpValues = GenerateHpDistribution(pixels.Count, _difficultyMode, seed);

                int pixelIndex = 0;

                foreach (int hp in hpValues)
                {
                    int safeIndex = Math.Min(pixelIndex, pixels.Count - 1);

                    int depth = pixels[safeIndex].Depth;

                    tanksData.Add(
                        new TankData(
                            color,
                            hp,
                            0,
                            depth
                        )
                    );

                    pixelIndex += hp;
                }

                colorIndex++;
            }

            tanksData = _difficultyGenerator.Generate(tanksData, _difficultyMode, LanesCount, _seed);

            DataGenerated?.Invoke(tanksData);
        }

        private Dictionary<Color, List<PixelData>> GroupPixelsByColor(List<PixelData> pixelsData)
        {
            Dictionary<Color, List<PixelData>> pixelsByColor = new();

            foreach (PixelData pixel in pixelsData)
            {
                if (pixelsByColor.TryGetValue(pixel.Color, out List<PixelData> pixels) == false)
                {
                    pixels = new List<PixelData>();
                    pixelsByColor.Add(pixel.Color, pixels);
                }

                pixels.Add(pixel);
            }

            return pixelsByColor;
        }

        private List<int> GenerateHpDistribution(int totalPixels, DifficultyMode difficultyMode, int seed)
        {
            DifficultySettings settings = _difficultySettingsProvider.Get(difficultyMode);
            DeterministicRandom random = new DeterministicRandom(seed);

            List<int> result = new();

            int remaining = totalPixels;

            while (remaining > 0)
            {
                if (remaining < 10)
                {
                    result.Add(remaining);
                    break;
                }

                int hp = SelectHp(remaining, settings, random);

                result.Add(hp);

                remaining -= hp;
            }

            return result;
        }


        private int SelectHp(int remaining, DifficultySettings settings, DeterministicRandom random)
        {
            List<int> candidates = new();
            List<int> weights = new();

            for (int i = 0; i < settings.HpValues.Length; i++)
            {
                int hp = settings.HpValues[i];

                if (hp > remaining)
                    continue;

                int residual = remaining - hp;

                if (residual > 0 && residual < MinResidualHp)
                    continue;

                candidates.Add(hp);
                weights.Add(settings.HpWeights[i]);
            }

            if (candidates.Count == 0)
                return Math.Min(50, remaining);

            int totalWeight = weights.Sum();

            int roll = random.NextInt(0, totalWeight);

            int accumulated = 0;

            for (int i = 0; i < candidates.Count; i++)
            {
                accumulated += weights[i];

                if (roll < accumulated)
                    return candidates[i];
            }

            return candidates[^1];
        }
    }
}