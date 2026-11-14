using UnityEngine;

namespace _Game.Code.TankComponents
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Tank))]
    public class TankAnimator : MonoBehaviour
    {
        private const float FireRate = 0.05f;
        
        private static readonly int ShootKey = Animator.StringToHash("Shoot");
        private static readonly int ShootSpeedKey = Animator.StringToHash("ShootSpeed");
        private static readonly int DieKey = Animator.StringToHash("Die");

        [SerializeField] private AnimationClip _shootClip;
        
        private Animator _animator;
        private Tank _tank;

        private float _shootClipSpeed;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _tank = GetComponent<Tank>();
            
            _shootClipSpeed = _shootClip.length / FireRate;
        }

        private void OnEnable()
        {
            _tank.Shot += OnShot;
            _tank.Died += OnDied;
        }

        private void OnDisable()
        {
            _tank.Shot -= OnShot;
            _tank.Died -= OnDied;
        }

        public void OnDieAnimationComplete()
        {
            _tank.Die();
        }

        private void OnShot()
        {
            _animator.SetFloat(ShootSpeedKey, _shootClipSpeed);
            _animator.SetTrigger(ShootKey);
        }

        private void OnDied(Tank tank)
        {
            _animator.SetTrigger(DieKey);
        }
    }
}