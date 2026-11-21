using System;
using System.Collections;
using _Game.Code.ConfigurationProvider;
using _Game.Code.Extensions;
using SplineMesh;
using UnityEngine;
using VContainer;

namespace _Game.Code.TankComponents
{
    public class TankMover : MonoBehaviour
    {
        private float _speed;
        private float _speedModifier;

        private Spline _spline;
        private TankConfigurationProvider _tankConfigurationProvider;

        private float _currentSpeed;
        private float _passedDistance;
        private Coroutine _moveCoroutine;
        private bool _isFirstCirclePassed;
        private bool _isLoop;
        
        public bool IsMoving => _moveCoroutine != null;
        public bool IsLoop => _isLoop;

        public event Action<float> DistancePassed;
        public event Action CirclePassed;

        [Inject]
        public void Construct(Spline spline, TankConfigurationProvider tankConfigurationProvider)
        {
            _spline = spline ?? throw new ArgumentNullException(nameof(spline));
            _tankConfigurationProvider = tankConfigurationProvider 
                                         ?? throw new ArgumentNullException(nameof(tankConfigurationProvider));

            _speed = _tankConfigurationProvider.TankConfiguration.TankSpeed;
            _speedModifier = _tankConfigurationProvider.TankConfiguration.SpeedModifier;
        }

        public void StartMove()
        {
            _passedDistance = 0f;

            this.StopCurrentCoroutine(ref _moveCoroutine);
            _moveCoroutine = StartCoroutine(Move());
        }

        public void StopMove() =>
            this.StopCurrentCoroutine(ref _moveCoroutine);

        public void StartLoopMove()
        {
            _isLoop = true;
            _speed *= _speedModifier;
        }

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
            while (_passedDistance < _spline.Length)
            {
                _passedDistance += _speed * Time.deltaTime;
                _passedDistance = Mathf.Min(_passedDistance, _spline.Length);

                DistancePassed?.Invoke(_passedDistance);

                CurveSample curveSample = _spline.GetSampleAtDistance(_passedDistance);

                transform.position = curveSample.location;

                yield return null;
            }

            CirclePassed?.Invoke();
        }
    }
}