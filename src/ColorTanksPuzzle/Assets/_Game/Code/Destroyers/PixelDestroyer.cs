using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Pixels;
using _Game.Code.Spawners;
using UnityEngine;
using VContainer;

namespace _Game.Code.Destroyers
{
    public class PixelDestroyer : IDisposable
    {
        private readonly PixelArtSpawner _pixelArtSpawner;
        
        private List<Pixel> _pixels;
        private Dictionary<Color, List<Pixel>> _pixelsByColor;

        [Inject]
        public PixelDestroyer(PixelArtSpawner pixelArtSpawner)
        {
            _pixelArtSpawner = pixelArtSpawner ??  throw new ArgumentNullException(nameof(pixelArtSpawner));

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

            foreach (Pixel pixel in pixels) 
                pixel.Died += OnDied;
        }
        
        private void OnDied(Pixel pixel)
        {
            pixel.Died -= OnDied;

            Destroy(pixel);
        }
    }
}