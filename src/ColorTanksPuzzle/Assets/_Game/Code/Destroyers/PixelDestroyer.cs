using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.PersistenceProgress;
using _Game.Code.Pixels;
using _Game.Code.Spawners;
using UnityEngine;
using VContainer;

namespace _Game.Code.Destroyers
{
    public class PixelDestroyer : IDisposable
    {
        private readonly PixelArtSpawner _pixelArtSpawner;
        private readonly CoinsCounter _coinsCounter;

        private List<Pixel> _pixels;
        private Dictionary<Color, List<Pixel>> _pixelsByColor;
        
        public event Action AllPixelsDestroyed;

        [Inject]
        public PixelDestroyer(PixelArtSpawner pixelArtSpawner, CoinsCounter coinsCounter)
        {
            _pixelArtSpawner = pixelArtSpawner ??  throw new ArgumentNullException(nameof(pixelArtSpawner));
            _coinsCounter = coinsCounter ?? throw new ArgumentNullException(nameof(coinsCounter));

            _pixelArtSpawner.ArtGenerated += OnArtGenerated;
        }

        public void Dispose()
        {
            _pixelArtSpawner.ArtGenerated -= OnArtGenerated;
        }

        public void Destroy(Pixel pixel)
        {
            _pixels.Remove(pixel);
            _pixelsByColor[pixel.Color].Remove(pixel);
            
            pixel.Die();
            
            if (_pixels.Count == 0) 
                AllPixelsDestroyed?.Invoke();
            
            _coinsCounter.IncreaseMoney();
        }
        
        public void DestroyAllByColor(Color color)
        {
            _pixels.FindAll(x => x.Color.Equals(color)).ForEach(Destroy);
        }

        public bool TryGetPixelsByColor(Color color, out List<Pixel> pixels)
        {
            return _pixelsByColor.TryGetValue(color, out pixels);
        }

        private void OnArtGenerated(List<Pixel> pixels)
        {
            _pixels = pixels;
            
            _pixelsByColor = pixels
                .GroupBy(x => x.Color)
                .ToDictionary(x => x.Key, x => x.ToList());
        }
    }
}