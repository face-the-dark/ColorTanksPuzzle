using System;
using System.Linq;
using _Game.Code.Generators.Data;
using _Game.Code.Infrastructure;
using _Game.Code.Providers;
using _Game.Code.Spawners;
using _Game.Code.WaitingAreaComponents;
using DG.Tweening;
using SplineMesh;
using UnityEngine;

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
        private WaitingArea _waitingArea;
        private SpawningLane _lane;

        private bool _isFirstCirclePassed;
        private bool _isMoving;
        private bool _isLoopMoving;
        private bool _isBlocked = true;
        private Sequence _moveTweenerCore;

        public bool IsMoving => _isMoving;
        public bool IsBlocked => _isBlocked;
        public int Hp => _tankHealth.Value;
        public Color Color => _tankShooter.Color;

        public event Action Shot;
        public event Action<int> HealthValueChanged;
        public event Action<Tank> MovingStopped;
        public event Action<Tank> Died;

        public void Initialize
        (
            Spline spline,
            TankData tankData,
            WaitingArea waitingArea,
            SpawningLane spawningLane,
            GameFactory gameFactory,
            TankConfigurationProvider tankConfigurationProvider
        )
        {
            _spline = spline;
            _tankMover.Initialize(spline, tankConfigurationProvider);
            _tankRotator.Initialize(spline);
            _tankShooter.Initialize(tankData.Color, gameFactory);
            _tankHealth.Initialize(tankData.Hp);
            _tankDyer.Initialize(tankData);

            _waitingArea = waitingArea;
            _lane = spawningLane;

            _tankShooter.Shot += OnShot;
            _tankHealth.ValueChanged += OnHealthValueChanged;
            _tankHealth.Died += Die;
        }

        private void OnHealthValueChanged(int value)
        {
            HealthValueChanged?.Invoke(value);
        }

        public void MoveToSpline()
        {
            if (_isMoving == false)
                RemoveFromWaitingAreaOrLane();

            _moveTweenerCore = transform
                .DOJump(_spline.nodes.First().Position, 20f, 1, 0.5f)
                .SetEase(Ease.Linear);

            _moveTweenerCore.onComplete += StartMove;
        }

        private void StartMove()
        {
            _moveTweenerCore.onComplete -= StartMove;

            _tankMover.CurrentLengthPercentageIncreased += OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed += OnCirclePassed;

            _tankMover.StartMove();
            _tankRotator.StartRotate();
            _tankShooter.StartShoot();

            _isMoving = true;
        }

        public void RemoveFromWaitingAreaOrLane()
        {
            if (_isFirstCirclePassed && _isMoving == false)
                _waitingArea.Remove(this);
            else
                _lane.Remove(this);
        }

        public void StartLoopMove()
        {
            _isLoopMoving = true;
            _tankMover.IncreaseSpeed();
        }

        public void StartShoot() =>
            _tankShooter.StartShoot();

        public void StopShoot() =>
            _tankShooter.StopShoot();

        public void Unblock() =>
            _isBlocked = false;

        public void FreezeMoving() =>
            _tankMover.ZeroingSpeed();

        public void UnfreezeMoving() =>
            _tankMover.UnZeroingSpeed();

        public void Die()
        {
            _tankHealth.Died -= Die;
            _tankHealth.ValueChanged -= OnHealthValueChanged;
            _tankShooter.Shot -= OnShot;
            _tankMover.CurrentLengthPercentageIncreased -= OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed -= OnCirclePassed;

            _isMoving = false;
            
            MovingStopped?.Invoke(this);
            Died?.Invoke(this);
            
            _tankMover.StopMove();
            _tankRotator.StopRotate();
            _tankShooter.StopShoot();
            _tankDeath.Die();
        }

        private void OnCurrentLengthPercentageIncreased(float currentLengthPercentage) =>
            _tankRotator.UpdateCurrentLengthPercentage(currentLengthPercentage);

        private void OnCirclePassed()
        {
            _tankMover.CurrentLengthPercentageIncreased -= OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed -= OnCirclePassed;

            if (_isLoopMoving)
            {
                MoveToSpline();
            }
            else
            {
                if (_isFirstCirclePassed == false)
                    _isFirstCirclePassed = true;

                _waitingArea.Add(this);

                _tankMover.StopMove();
                _tankRotator.StopRotate();
                _tankShooter.StopShoot();

                _isMoving = false;

                MovingStopped?.Invoke(this);
            }
        }

        private void OnShot()
        {
            _tankHealth.Decrease();
            
            Shot?.Invoke();
        }
    }
}