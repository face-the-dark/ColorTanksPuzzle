using UnityEngine;

namespace _Game.Code
{
    public class TankShooter : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootMaxDistance = 20f;
        [SerializeField] private LayerMask _pixelLayer;
        [SerializeField] private Bullet _bulletPrefab;

        private Color _color;
        private Pixel _currentPixel;
        private bool _canShoot = true;

        private void Start()
        {
            _color = GetComponent<Renderer>().material.color;
        }

        private void Update()
        {
            Shoot();
        }

        public void CanShoot()
        {
            _canShoot = true;
        }

        public void CannotShoot()
        {
            _canShoot = false;
        }

        private void Shoot()
        {
            Ray ray = new Ray(_shootPoint.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _shootMaxDistance, _pixelLayer) && _canShoot)
            {
                if (hit.collider.TryGetComponent(out Pixel pixel))
                {
                    if (pixel.Color == _color 
                        && pixel.Equals(_currentPixel) == false 
                        && pixel.IsWillBeDestroyed == false)
                    {
                        SpawnBullet();
                        pixel.MarkForDestruction();
                        _currentPixel = pixel;
                    }
                }
            }
        }

        private void SpawnBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
            bullet.Fly(transform.forward);
        }
    }
}