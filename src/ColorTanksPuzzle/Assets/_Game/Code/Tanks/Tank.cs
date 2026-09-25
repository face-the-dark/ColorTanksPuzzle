using System;
using _Game.Code.Data;
using _Game.Code.Spawners;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankMover _tankMover;
        [SerializeField] private TankRotator _tankRotator;
        [SerializeField] private TankShooter _tankShooter;
        [SerializeField] private TankDyer _tankDyer;

        private WaitingArea _waitingArea;
        private SpawningLane _lane;

        private bool _isFirstCirclePassed;
        private bool _isMoving;
        private bool _isLoopMoving;
        private bool _isBlocked = true;

        public bool IsMoving => _isMoving;
        public bool IsBlocked => _isBlocked;
        public int Hp => _tankShooter.Hp;
        public Color Color => _tankShooter.Color;

        public event Action<Tank> MovingStopped;
        public event Action<Tank> Died;

        public void Initialize
        (
            SplineContainer spline,
            TankData tankData,
            WaitingArea waitingArea,
            SpawningLane spawningLane
        )
        {
            _tankMover.Initialize(spline);
            _tankRotator.Initialize(spline);
            _tankShooter.Initialize(tankData);
            _tankDyer.Initialize(tankData);

            _waitingArea = waitingArea;
            _lane = spawningLane;

            _tankShooter.Died += Die;
        }

        public void MoveToSpline()
        {
            if (_isMoving == false) 
                RemoveFromWaitingAreaOrLane();

            _tankMover.CurrentLengthPercentageIncreased += OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed += OnCirclePassed;

            _tankMover.StartMove();
            _tankRotator.StartRotate();
            _tankShooter.StartShoot();

            _isMoving = true;
        }

        public void RemoveFromWaitingAreaOrLane()
        {
            if (_isFirstCirclePassed)
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

        public void FreezeMoving()
        {
            _tankMover.ZeroingSpeed();
        }

        public void UnfreezeMoving()
        {
            _tankMover.UnZeroingSpeed();
        }  

        public void Die()
        {
            _tankShooter.Died -= Die;
            _tankMover.CurrentLengthPercentageIncreased -= OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed -= OnCirclePassed;

            MovingStopped?.Invoke(this);
            Died?.Invoke(this);

            Destroy(gameObject);
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
    }
}