using _Game.Code.Data;
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

        public void Initialize(SplineContainer spline, TankData tankData)
        {
            _tankMover.Initialize(spline);
            _tankRotator.Initialize(spline);
            _tankShooter.Initialize(tankData);
            _tankDyer.Initialize(tankData);
        }

        public void MoveToSpline()
        {
            _tankMover.StartMove();
            _tankRotator.StartRotate();
            _tankShooter.StartShoot();
            
            _tankMover.CurrentLengthPercentageIncreased += OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed += OnCirclePassed;
        }

        private void OnCurrentLengthPercentageIncreased(float currentLengthPercentage)
        {
            _tankRotator.UpdateCurrentLengthPercentage(currentLengthPercentage);
        }

        private void OnCirclePassed()
        {
            _tankMover.StopMove();
            _tankRotator.StopRotate();
            _tankShooter.StopShoot();
            
            _tankMover.CurrentLengthPercentageIncreased -= OnCurrentLengthPercentageIncreased;
            _tankMover.CirclePassed -= OnCirclePassed;
        }
    }
}