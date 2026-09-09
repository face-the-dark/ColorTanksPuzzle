using UnityEngine;

namespace _Game.Code
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Pixel pixel))
            {
                Destroy(pixel.gameObject);
                Destroy(gameObject);
            }
        }

        public void Fly(Vector3 direction)
        {
            _rigidbody.AddForce(_speed * direction, ForceMode.Impulse);
        }
    }
}