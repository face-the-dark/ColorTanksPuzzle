using System;
using System.Collections;
using _Game.Code.Bullets;
using _Game.Code.Data;
using _Game.Code.Extensions;
using _Game.Code.Pixels;
using UnityEngine;

namespace _Game.Code.Tanks
{
    public class TankShooter : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootMaxDistance = 20f;
        [SerializeField] private LayerMask _pixelLayer;
        [SerializeField] private Bullet _bulletPrefab;

        private Color _color;
        private int _Hp;

        private Coroutine _shootCoroutine;
        private Pixel _currentPixel;
        private bool _isShooting;

        public event Action Died;
        public event Action<int> HpChanged;

        public void Initialize(TankData tankData)
        {
            _color = tankData.Color;
            _Hp = tankData.Hp;
        }

        private void Start()
        {
            HpChanged?.Invoke(_Hp);
        }

        public void StartShoot()
        {
            StopShoot();
            _isShooting = true;
            _shootCoroutine = StartCoroutine(Shoot());
        }

        public void StopShoot()
        {
            _isShooting = false;
            this.StopCurrentCoroutine(ref _shootCoroutine);
        }
        
        private IEnumerator Shoot()
        {
            while (_isShooting)
            {
                Ray ray = new Ray(_shootPoint.position, transform.forward);
                
                if (Physics.Raycast(ray, out RaycastHit hit, _shootMaxDistance, _pixelLayer))
                {
                    if (hit.collider.TryGetComponent(out Pixel pixel))
                    {
                        if (CanDestroyPixel(pixel))
                        {
                            SpawnBullet();
                            pixel.MarkForDestruction();
                            _currentPixel = pixel;

                            ReduceHp();
                        }
                    }
                }
                
                yield return null;
            }
        }

        private bool CanDestroyPixel(Pixel pixel)
        {
            return pixel.Color == _color
                   && pixel.Equals(_currentPixel) == false
                   && pixel.IsWillBeDestroyed == false;
        }

        private void SpawnBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            bullet.Fly(transform.forward);
        }

        private void ReduceHp()
        {
            _Hp--;

            if (_Hp <= 0) 
                Died?.Invoke();
            
            HpChanged?.Invoke(_Hp);
        }
    }
}