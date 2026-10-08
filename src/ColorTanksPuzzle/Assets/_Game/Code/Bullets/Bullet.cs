using _Game.Code.Destroyers;
using _Game.Code.Pixels;
using UnityEngine;

namespace _Game.Code.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;

        private PixelDestroyer _pixelDestroyer;
        
        private Rigidbody _rigidbody;

        public void Construct(PixelDestroyer pixelDestroyer) => 
            _pixelDestroyer = pixelDestroyer;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody>();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Pixel pixel))
            {
                _pixelDestroyer.Destroy(pixel);
                
                Die();
            }
        }

        public void AddForce(Vector3 direction) => 
            _rigidbody.AddForce(_speed * direction, ForceMode.Impulse);

        private void Die() => 
            Destroy(gameObject);
    }
}