using System;
using _Game.Code.Data;
using UnityEngine;

namespace _Game.Code.Pixels
{
    public class Pixel : MonoBehaviour
    {
        private Color _color;
        private bool _isWillBeDestroyed;

        public event Action<Pixel> PixelWillBeDestroyed;
        
        public Color Color => _color;
        public bool IsWillBeDestroyed => _isWillBeDestroyed;

        public void Initialize(PixelData pixelData) => 
            _color = pixelData.Color;

        public void MarkForDestruction()
        {
            _isWillBeDestroyed = true;
            
            PixelWillBeDestroyed?.Invoke(this);
        }

        public void Die()
        {
            if (_isWillBeDestroyed)
                Destroy(gameObject);
        }
    }
}