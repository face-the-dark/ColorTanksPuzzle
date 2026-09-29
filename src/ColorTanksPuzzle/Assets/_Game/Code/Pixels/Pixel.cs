using System;
using _Game.Code.Generators.Data;
using UnityEngine;

namespace _Game.Code.Pixels
{
    public class Pixel : MonoBehaviour
    {
        private Color _color;
        private bool _isWillBeDestroyed;

        public event Action<Pixel> Died;
        
        public Color Color => _color;
        public bool IsWillBeDestroyed => _isWillBeDestroyed;

        public void Initialize(PixelData pixelData) =>
            _color = pixelData.Color;

        public void MarkForDestruction() => 
            _isWillBeDestroyed = true;

        public void Die()
        {
            Died?.Invoke(this);
            
            Destroy(gameObject);
        }
    }
}