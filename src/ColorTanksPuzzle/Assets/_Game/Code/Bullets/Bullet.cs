using System.Collections;
using _Game.Code.Destroyers;
using _Game.Code.Extensions;
using _Game.Code.Pixels;
using UnityEngine;

namespace _Game.Code.Bullets
{
    [RequireComponent(typeof(BulletDyer))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;
        
        private BulletDyer _bulletDyer;
        
        private PixelDestroyer _pixelDestroyer;

        private Coroutine _flyCoroutine;
        private bool _isFlying;

        public void Construct(PixelDestroyer pixelDestroyer) =>
            _pixelDestroyer = pixelDestroyer;

        private void Awake()
        {
            _bulletDyer = GetComponent<BulletDyer>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Pixel pixel))
            {
                if (Equals(pixel.DestructionBullet))
                {
                    _isFlying = false;
                    
                    _pixelDestroyer.Destroy(pixel);

                    Die();
                }
            }
        }

        public void StartFly(Vector3 targetPosition, Color color)
        {
            this.StopCurrentCoroutine(ref _flyCoroutine);
            _flyCoroutine = StartCoroutine(Fly(targetPosition));
            
            _bulletDyer.ApplyColor(color);
        }

        private IEnumerator Fly(Vector3 targetPosition)
        {
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
            
            _isFlying = true;
            
            while (_isFlying)
            {
                transform.position = Vector3.MoveTowards
                (
                    transform.position,
                    targetPosition,
                    _speed * Time.deltaTime
                );

                yield return null;
            }
        }

        private void Die() =>
            Destroy(gameObject);
    }
}