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

            List<KeyValuePair<Color, List<PixelData>>> orderedPixelsDataByColor = OrderPixelsDataByColor(pixelsData);

            foreach (KeyValuePair<Color, List<PixelData>> pixelsDataByColor in orderedPixelsDataByColor)
            {
                Color color = pixelsDataByColor.Key;
                List<PixelData> dataByColor = pixelsDataByColor.Value;
                
                List<PixelData> orderedPixelsDataByDepth = OrderPixelsDataByDepth(dataByColor);

                GenerateTanksDataForColor(color, orderedPixelsDataByDepth, tanks);
            }

            tanks = _orderGenerator.OrderByDepth(tanks);
            
            _laneDistributor.Distribute(tanks);

            DataGenerated?.Invoke(tanks);
        }

        private List<KeyValuePair<Color, List<PixelData>>> OrderPixelsDataByColor(List<PixelData> pixelsData)
        {
            return pixelsData
                .GroupBy(p => p.Color)
                .OrderBy(g => g.Key.GetHashCode())
                .Select(g => new KeyValuePair<Color, List<PixelData>>(g.Key, g.ToList()))
                .ToList();
        }

        private List<PixelData> OrderPixelsDataByDepth(List<PixelData> pixelsData)
        {
            return pixelsData
                .OrderBy(pixel => pixel.Depth)
                .ToList();
        }

        private void GenerateTanksDataForColor(Color color, List<PixelData> pixelData, List<TankData> tanksData)
        {
            List<int> hpValues = _hpGenerator.GenerateTanksHpByPixelsCount(pixelData.Count);

            int pixelIndex = 0;

            foreach (int hp in hpValues)
            {
                PixelData firstPixel = pixelData[pixelIndex];

                TankData tank = new TankData(color, hp, firstPixel.Depth);

                tanksData.Add(tank);

                pixelIndex += hp;
            }
        }
    }
}