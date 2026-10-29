using System;
using System.Collections;
using _Game.Code.Extensions;
using _Game.Code.Providers;
using SplineMesh;
using UnityEngine;

namespace _Game.Code.Tanks
{
    public class TankMover : MonoBehaviour
    {
        private float _speed;
        private float _speedModifier;

        private Spline _spline;
        private TankConfigurationProvider _tankConfigurationProvider;

        private float _currentSpeed;
        private float _currentSplineRate;
        private Coroutine _moveCoroutine;

        public event Action<float> CurrentLengthPercentageIncreased;
        public event Action CirclePassed;

        public void Initialize(Spline spline, TankConfigurationProvider tankConfigurationProvider)
        {
            _spline = spline;
            _tankConfigurationProvider = tankConfigurationProvider;

            _speed = _tankConfigurationProvider.TankConfiguration.TankSpeed;
            _speedModifier = _tankConfigurationProvider.TankConfiguration.SpeedModifier;
        }

        public void StartMove()
        {
            _currentSplineRate = 0f;

            this.StopCurrentCoroutine(ref _moveCoroutine);
            _moveCoroutine = StartCoroutine(Move());
        }

        public void StopMove() =>
            this.StopCurrentCoroutine(ref _moveCoroutine);

        public void IncreaseSpeed() =>
            _speed *= _speedModifier;

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
            while (_currentSplineRate < _spline.Length)
            {
                _currentSplineRate += _speed * Time.deltaTime;
                _currentSplineRate = Mathf.Min(_currentSplineRate, _spline.Length);

                CurrentLengthPercentageIncreased?.Invoke(_currentSplineRate);

                CurveSample curveSample = _spline.GetSampleAtDistance(_currentSplineRate);

                transform.position = curveSample.location;

                yield return null;
            }

            CirclePassed?.Invoke();
        }
    }
}