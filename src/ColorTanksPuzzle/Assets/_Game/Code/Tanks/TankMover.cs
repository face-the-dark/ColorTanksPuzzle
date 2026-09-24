using System;
using System.Collections;
using _Game.Code.Extensions;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class TankMover : MonoBehaviour
    {
        private const float MaxSplineLengthPercentage = 1f;
        
        [SerializeField] private float _speed = 0.2f;
        [SerializeField] private float _speedModifier = 2f;

        private SplineContainer _spline;

        private float _currentSpeed;
        private float _currentLengthPercentage;
        private Coroutine _moveCoroutine;

        public event Action<float> CurrentLengthPercentageIncreased;
        public event Action CirclePassed;
        
        public void Initialize(SplineContainer spline) => 
            _spline = spline;

        public void StartMove()
        {
            _currentLengthPercentage = 0f;

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
            if  (_speed == 0f)
                _speed = _currentSpeed;
        }

        private IEnumerator Move()
        {
            while (_currentLengthPercentage < MaxSplineLengthPercentage)
            {
                _currentLengthPercentage += _speed * Time.deltaTime;
                
                CurrentLengthPercentageIncreased?.Invoke(_currentLengthPercentage);
                
                transform.position = _spline.EvaluatePosition(_currentLengthPercentage);

                yield return null;
            }
            
            CirclePassed?.Invoke();
        }
    }
}