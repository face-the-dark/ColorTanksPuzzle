using System;
using _Game.Code.Bullets;
using _Game.Code.Generators.Data;
using _Game.Code.Spawners;
using UnityEngine;

namespace _Game.Code.Pixels
{
    [RequireComponent(typeof(PixelAnimator))]
    public class Pixel : MonoBehaviour
    {
        private PixelAnimator _pixelAnimator;
        
        private Color _color;
        private CoinSpawner _coinSpawner;
        
        private bool _isWillBeDestroyed;
        private Bullet _destructionBullet;

        public Color Color => _color;
        public bool IsWillBeDestroyed => _isWillBeDestroyed;
        public Bullet DestructionBullet => _destructionBullet;

        public void Initialize(PixelData pixelData, CoinSpawner coinSpawner)
        {
            _color = pixelData.Color;
            _coinSpawner = coinSpawner;
        }

        private void Awake()
        {
            _pixelAnimator = GetComponent<PixelAnimator>();
        }

        public void MarkForDestruction(Bullet bullet)
        {
            _isWillBeDestroyed = true;
            _destructionBullet = bullet;
        }

        public void Die()
        {
            _coinSpawner.Spawn(transform.position);
            
            _pixelAnimator.Die();
        }
    }
}