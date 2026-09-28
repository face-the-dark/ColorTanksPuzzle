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

        [Inject]
        public PixelDestroyer(PixelArtSpawner pixelArtSpawner)
        {
            _pixelArtSpawner = pixelArtSpawner ??  throw new ArgumentNullException(nameof(pixelArtSpawner));

            _pixelArtSpawner.ArtGenerated += OnArtGenerated;
        }

        public event Action<Pixel> PixelDestroyed;

        public void Dispose()
        {
            _pixelArtSpawner.ArtGenerated -= OnArtGenerated;
        }

        public void Destroy(Pixel pixel)
        {
            _pixels.Remove(pixel);
            pixel.MarkForDestruction();
            
            PixelDestroyed?.Invoke(pixel);
            
            pixel.Die();
        }
        
        public void DestroyAllByColor(Color color)
        {
            _pixels.FindAll(x => x.Color.Equals(color)).ForEach(pixel =>
            {
                _pixels.Remove(pixel);
                pixel.MarkForDestruction();
            
                PixelDestroyed?.Invoke(pixel);
            
                pixel.Die();
            });
        }

        private void OnArtGenerated(List<Pixel> pixels)
        {
            _pixels = pixels;   
        }
    }
}