using System;
using System.Linq;
using _Game.Code.Generators.Data;
using SplineMesh;
using UnityEngine;
using VContainer;

namespace _Game.Code.TankComponents
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankMover _tankMover;
        [SerializeField] private TankRotator _tankRotator;
        [SerializeField] private TankShooter _tankShooter;
        [SerializeField] private TankHealth _tankHealth;
        [SerializeField] private TankDeath _tankDeath;
        [SerializeField] private TankDyer _tankDyer;

        private Spline _spline;
        private ITankJumpAnimator _tankJumpAnimator;

        private bool _isBlocked = true;

        public bool IsMoving => _tankMover.IsMoving;
        public bool IsBlocked => _isBlocked;
        public int Hp => _tankHealth.Value;
        public Color Color => _tankShooter.Color;

        public event Action Shot;
        public event Action<int> HealthValueChanged;
        public event Action HealthValueEnded;
        public event Action<Tank> MovingStopped;
        public event Action<Tank> CirclePassed;
        public event Action<Tank> Died;

        [Inject]
        public void Construct(Spline spline, ITankJumpAnimator tankJumpAnimator)
        {
            _spline = spline ?? throw new ArgumentNullException(nameof(spline));
            _tankJumpAnimator = tankJumpAnimator ?? throw new ArgumentNullException(nameof(tankJumpAnimator));
        }

        public void Initialize(TankData tankData)
        {
            if (tankData == null)
                throw new ArgumentNullException(nameof(tankData));
            
            _tankShooter.Initialize(tankData.Color);
            _tankHealth.Initialize(tankData.Hp);
            _tankDyer.Initialize(tankData.Color);
        }
        
        private void OnDestroy()
        {
            _tankJumpAnimator?.Kill();
        }

        public void MoveToSpline()
        {
            _tankJumpAnimator.JumpTo(
                transform,
                _spline.nodes.First().Position,
                power: 20f,
                duration: 0.5f,
                onComplete: StartMove);
        }

        private void StartMove()
        {
            _tankShooter.Shot += OnShot;
            _tankHealth.ValueChanged += OnHealthValueChanged;
            _tankHealth.ValueEnded += OnHealthValueEnded;
            _tankMover.DistancePassed += OnDistancePassed;
            _tankMover.CirclePassed += OnCirclePassed;

            _tankMover.StartMove();
            _tankRotator.StartRotate();
            _tankShooter.StartShoot();
        }

        private void StopMove()
        {
            _tankMover.StopMove();
            _tankRotator.StopRotate();
            _tankShooter.StopShoot();

            _tankShooter.Shot -= OnShot;
            _tankHealth.ValueChanged -= OnHealthValueChanged;
            _tankHealth.ValueEnded -= OnHealthValueEnded;
            _tankMover.DistancePassed -= OnDistancePassed;
            _tankMover.CirclePassed -= OnCirclePassed;

            MovingStopped?.Invoke(this);
        }

        public void StartLoopMove() =>
            _tankMover.StartLoopMove();

        public void StartShoot() =>
            _tankShooter.StartShoot();

        public void StopShoot() =>
            _tankShooter.StopShoot();

        public void Unblock() =>
            _isBlocked = false;

        public void Block() => 
            _isBlocked = true;

        public void FreezeMoving() =>
            _tankMover.ZeroingSpeed();

        public void UnfreezeMoving() =>
            _tankMover.UnZeroingSpeed();

        public void Die()
        {
            StopMove();

            _tankDeath.Die();

            Died?.Invoke(this);
        }

        private void OnDistancePassed(float distance) =>
            _tankRotator.UpdatePassedDistance(distance);

        private void OnCirclePassed()
        {
            StopMove();

            if (_tankMover.IsLoop)
                StartMove();

            CirclePassed?.Invoke(this);
        }

        private void OnShot()
        {
            _tankHealth.Decrease();

            Shot?.Invoke();
        }

        private void OnHealthValueChanged(int value)
        {
            HealthValueChanged?.Invoke(value);
        }

        private void OnHealthValueEnded()
        {
            StopMove();

            HealthValueEnded?.Invoke();
        }
    }
}