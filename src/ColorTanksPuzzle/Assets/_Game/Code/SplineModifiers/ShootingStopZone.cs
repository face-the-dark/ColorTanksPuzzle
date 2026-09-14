using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.SplineModifiers
{
    public class ShootingStopZone : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tank tankShooter))
                tankShooter.StopShoot();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Tank tankShooter))
                tankShooter.StartShoot();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(_collider.transform.position, _collider.size);
        }
    }
}