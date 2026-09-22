using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Data;
using UnityEngine;

namespace _Game.Code.Generators.Tanks
{
    public class TanksDataGenerator : IDisposable
    {
        private readonly PixelArtGenerator _pixelArtGenerator;
        private readonly TankHpGenerator _hpGenerator;
        private readonly TankOrderGenerator _orderGenerator;
        private readonly TankLaneDistributor _laneDistributor;

        public event Action<List<TankData>> DataGenerated;

        public TanksDataGenerator
        (
            PixelArtGenerator pixelArtGenerator,
            TankHpGenerator hpGenerator,
            TankOrderGenerator orderGenerator,
            TankLaneDistributor laneDistributor
        )
        {
            _pixelArtGenerator = pixelArtGenerator;
            _hpGenerator = hpGenerator;
            _orderGenerator = orderGenerator;
            _laneDistributor = laneDistributor;

            _pixelArtGenerator.ArtGenerated += GenerateTanksData;
        }

        public void Dispose() =>
            _pixelArtGenerator.ArtGenerated -= GenerateTanksData;

        private void GenerateTanksData(List<PixelData> pixelsData)
        {
            List<TankData> tanks = new();
            
            List<KeyValuePair<Color, List<PixelData>>> orderedPixelsDataByColor = OrderPixelsByColor(pixelsData);
            
            foreach (KeyValuePair<Color, List<PixelData>> pixelsDataByColor in orderedPixelsDataByColor)
            {
                Color color = pixelsDataByColor.Key;

                List<PixelData> colorPixels = pixelsDataByColor.Value
                    .OrderBy(pixel => pixel.Depth)
                    .ToList();

                GenerateTanksForColor(color, colorPixels, tanks);
            }

            tanks = _orderGenerator.OrderByDepth(tanks);
            tanks = _laneDistributor.Distribute(tanks);

            DataGenerated?.Invoke(tanks);
        }

        private List<KeyValuePair<Color, List<PixelData>>> OrderPixelsByColor(List<PixelData> pixelsData)
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

            return pixelsByColor
                .OrderBy(pair => pair.Key.GetHashCode())
                .ToList();
        }

        private void GenerateTanksForColor(Color color, List<PixelData> pixels, List<TankData> result)
        {
            List<int> hpValues = _hpGenerator.Generate(pixels.Count);

            int pixelIndex = 0;

            foreach (int hp in hpValues)
            {
                PixelData firstPixel = pixels[pixelIndex];

                TankData tank = new TankData(color, hp, firstPixel.Depth, 0);

                result.Add(tank);

                pixelIndex += hp;
            }
        }
    }
}