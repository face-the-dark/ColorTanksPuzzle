using UnityEngine;

namespace _Game.Code.TankComponents
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(TankShooter))]
    [RequireComponent(typeof(TankDeath))]
    public class TankAnimator : MonoBehaviour
    {
        private static readonly int ShootKey = Animator.StringToHash("Shoot");
        private static readonly int ShootSpeedKey = Animator.StringToHash("ShootSpeed");
        private static readonly int DieKey = Animator.StringToHash("Die");

        [SerializeField] private AnimationClip _shootClip;
        
        private Animator _animator;
        private TankShooter _tankShooter;
        private TankDeath _tankDeath;

        private float _shootClipSpeed;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _tankShooter = GetComponent<TankShooter>();
            _tankDeath = GetComponent<TankDeath>();
            
            _shootClipSpeed = _shootClip.length / _tankShooter.FireRate;
        }

        private void OnEnable()
        {
            _tankShooter.Shot += OnShot;
            _tankShooter.Died += OnDied;
        }

        private void OnDisable()
        {
            _tankShooter.Shot -= OnShot;
            _tankShooter.Died -= OnDied;
        }

        public void OnDieAnimationComplete()
        {
            _tankDeath.Die();
        }

        private void OnShot()
        {
            _animator.SetFloat(ShootSpeedKey, _shootClipSpeed);
            _animator.SetTrigger(ShootKey);
        }

        private void OnDied()
        {
            _animator.SetTrigger(DieKey);
        }
    }
}