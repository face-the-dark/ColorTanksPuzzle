using System;
using System.Collections.Generic;
using _Game.Code.Data;
using UnityEngine;

namespace _Game.Code.Generators
{
    public class TanksDataGenerator : MonoBehaviour
    {
        private const int LanesCount = 3;
        
        [SerializeField] private PixelArtGenerator _pixelArtGenerator;
        
        public event Action<List<TankData>> DataGenerated;

        private void OnEnable() => 
            _pixelArtGenerator.ArtGenerated += GenerateTanksData;

        private void OnDisable() => 
            _pixelArtGenerator.ArtGenerated -= GenerateTanksData;

        private void GenerateTanksData(List<PixelData> pixelsData)
        {
            List<TankData> tanksData = new();
            Dictionary<Color, int> colorsCount = CountColors(pixelsData);

            foreach (KeyValuePair<Color, int> colorCount in colorsCount)
            {
                Color color = colorCount.Key;
                int remainingColorCount = colorCount.Value;

                while (remainingColorCount > 0)
                {
                    int tankHp = Mathf.Min(20, remainingColorCount);
                    
                    tanksData.Add(new TankData(color, tankHp, tanksData.Count % LanesCount));

                    remainingColorCount -= tankHp;
                }
            }

            DataGenerated?.Invoke(tanksData);
        }

        private Dictionary<Color, int> CountColors(List<PixelData> pixelsData)
        {
            Dictionary<Color, int> counts = new Dictionary<Color, int>();

            foreach (PixelData pixel in pixelsData)
            {
                counts.TryAdd(pixel.Color, 0);
                counts[pixel.Color]++;
            }

            return counts;
        }
    }
}