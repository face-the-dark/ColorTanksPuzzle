using System;
using System.Collections;
using _Game.Code.Extensions;
using _Game.Code.Providers;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class TankMover : MonoBehaviour
    {
        private const float MaxSplineLengthPercentage = 1f;

        private float _speed;
        private float _speedModifier;

        private SplineContainer _spline;
        private TankConfigurationProvider _tankConfigurationProvider;
        private Reload _reload;

        private float _currentSpeed;
        private float _currentLengthPercentage;
        private Coroutine _moveCoroutine;

        public event Action<float> CurrentLengthPercentageIncreased;
        public event Action CirclePassed;

        public void Initialize(SplineContainer spline, TankConfigurationProvider tankConfigurationProvider, Reload reload)
        {
            _spline = spline;
            _tankConfigurationProvider = tankConfigurationProvider;
            _reload = reload;

            _speed = _tankConfigurationProvider.TankConfiguration.TankSpeed;
            _speedModifier = _tankConfigurationProvider.TankConfiguration.SpeedModifier;
        }

        public void StartMove()
        {
            _currentLengthPercentage = 0f;

            this.StopCurrentCoroutine(ref _moveCoroutine);
            _moveCoroutine = StartCoroutine(Move());
        }

        public void StopMove() =>
            this.StopCurrentCoroutine(ref _moveCoroutine);

        public void IncreaseSpeed() =>
            _speed /= _speedModifier;

        public void ZeroingSpeed()
        {
            if (_speed != 0f)
            {
                _currentSpeed = _speed;
                _speed = 0f;
            }
        }

        public void UnZeroingSpeed()
        {
            if (_speed == 0f)
                _speed = _currentSpeed;
        }

        private IEnumerator Move()
        {
            while (_currentLengthPercentage < MaxSplineLengthPercentage)
            {
                float speed;

                if (_speed == 0f)
                    speed = 0f;
                else
                    speed = 1f / _speed;

                _currentLengthPercentage += speed * Time.deltaTime;

                CurrentLengthPercentageIncreased?.Invoke(_currentLengthPercentage);

                transform.position = _spline.EvaluatePosition(_currentLengthPercentage);
                
                float distance = _spline.Spline.ConvertIndexUnit
                (
                    _currentLengthPercentage,
                    PathIndexUnit.Normalized,
                    PathIndexUnit.Distance
                );
                
                _reload.UpdateDistance(distance);

                yield return null;
            }

            CirclePassed?.Invoke();
        }
    }
}