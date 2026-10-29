using UnityEngine;

namespace _Game.Code.Tanks
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(TankShooter))]
    public class TankAnimator : MonoBehaviour
    {
        private static readonly int ShootKey = Animator.StringToHash("Shoot");
        private static readonly int ShootSpeedKey = Animator.StringToHash("ShootSpeed");

        [SerializeField] private AnimationClip _shootClip;
        
        private Animator _animator;
        private TankShooter _tankShooter;

        private float _shootClipSpeed;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _tankShooter = GetComponent<TankShooter>();
            
            _shootClipSpeed = _shootClip.length / _tankShooter.FireRate;
        }

        private void OnEnable()
        {
            _tankShooter.Shot += OnShot;
        }

        private void OnDisable()
        {
            _tankShooter.Shot -= OnShot;
        }

        private void OnShot()
        {
            _animator.SetFloat(ShootSpeedKey, _shootClipSpeed);
            _animator.SetTrigger(ShootKey);
        }
    }
}