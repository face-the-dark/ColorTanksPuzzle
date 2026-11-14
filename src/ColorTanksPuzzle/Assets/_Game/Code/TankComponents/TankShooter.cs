using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Bullets;
using _Game.Code.Extensions;
using _Game.Code.Infrastructure;
using _Game.Code.Pixels;
using UnityEngine;

namespace _Game.Code.TankComponents
{
    public class TankShooter : MonoBehaviour
    {
        private const int MaxRaycastHitsSize = 40;
        
        private readonly RaycastHit[] _raycastHits = new RaycastHit[MaxRaycastHitsSize];
        private readonly IComparer<RaycastHit> _distanceComparer =
            Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance));

        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootMaxDistance = 30f;
        [SerializeField] private LayerMask _pixelLayer;
        [SerializeField] private Bullet _bulletPrefab;

        private GameFactory _gameFactory;

        private Color _color;

        private Coroutine _shootCoroutine;
        private Pixel _currentPixel;
        private List<Pixel> _blockedPixels = new();
        private bool _isShooting;

        public event Action Shot;

        public Color Color => _color;

        public void Initialize(Color color, GameFactory gameFactory)
        {
            _color = color;

            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
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
            
            _blockedPixels = new List<Pixel>();
        }

        private IEnumerator Shoot()
        {
            while (_isShooting)
            {
                int hitsCount = Physics.RaycastNonAlloc
                (
                    _shootPoint.position,
                    _shootPoint.forward,
                    _raycastHits,
                    _shootMaxDistance,
                    _pixelLayer
                );

                Array.Sort(_raycastHits, 0, hitsCount, _distanceComparer);

                List<Pixel> pixelHits = new List<Pixel>();

                for (int i = 0; i < hitsCount; i++)
                    if (_raycastHits[i].collider && _raycastHits[i].collider.TryGetComponent(out Pixel pixelHit))
                        pixelHits.Add(pixelHit);
                
                if (pixelHits.Count > 0)
                {
                    Pixel firstPixel = pixelHits.FindAll(pixel => pixel.IsWillBeDestroyed == false).FirstOrDefault();

                    if (firstPixel && CanDestroyPixel(firstPixel) && _blockedPixels.Contains(firstPixel) == false)
                    {
                        Bullet bullet = SpawnBullet(firstPixel);

                        if (_currentPixel != firstPixel)
                            _blockedPixels = new();

                        _currentPixel = firstPixel;
                        firstPixel.MarkForDestruction(bullet);

                        _blockedPixels.AddRange(pixelHits.FindAll(pixel => pixel.IsWillBeDestroyed == false));

                        Shot?.Invoke();
                    }
                }

                yield return null;
            }
        }

        private bool CanDestroyPixel(Pixel pixel)
        {
            return pixel
                   && pixel.Color == _color
                   && pixel.Equals(_currentPixel) == false
                   && pixel.IsWillBeDestroyed == false
                   && pixel.DestructionBullet is null;
        }

        private Bullet SpawnBullet(Pixel pixel)
        {
            Vector3 bulletSpawnPosition = GetBulletSpawnPosition(pixel);
            Bullet bullet = _gameFactory.CreateBullet(bulletSpawnPosition);
            bullet.StartFly(pixel.transform.position, _color);

            return bullet;
        }

        private Vector3 GetBulletSpawnPosition(Pixel pixel)
        {
            if (pixel.TryGetComponent(out Collider colliderComponent))
            {
                Vector3 targetCenter = colliderComponent.bounds.center;

                Vector3 axisOrigin = _shootPoint.position;
                Vector3 axisDirection = _shootPoint.right.normalized;

                float distance = Vector3.Dot(targetCenter - axisOrigin, axisDirection);

                return axisOrigin + axisDirection * distance;
            }

            return _shootPoint.position;
        }
    }
}