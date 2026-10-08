using _Game.Code.Generators.Data;
using _Game.Code.Spawners;
using UnityEngine;

namespace _Game.Code.Pixels
{
    public class Pixel : MonoBehaviour
    {
        private Color _color;
        private CoinSpawner _coinSpawner;
        
        private bool _isWillBeDestroyed;

        public Color Color => _color;
        public bool IsWillBeDestroyed => _isWillBeDestroyed;

        public void Initialize(PixelData pixelData, CoinSpawner coinSpawner)
        {
            _color = pixelData.Color;
            _coinSpawner = coinSpawner;
        }

        public void MarkForDestruction() =>
            _isWillBeDestroyed = true;

        public void Die()
        {
            _coinSpawner.Spawn(transform.position);
            
            Destroy(gameObject);
        }
    }
}