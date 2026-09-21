using System;
using System.Collections;
using _Game.Code.Extensions;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class TankMover : MonoBehaviour
    {
        private const float MaxSplineLengtHpercentage = 1f;
        
        [SerializeField] private float _speed = 0.2f;
        [SerializeField] private float _speedModifier = 2f;

        private SplineContainer _spline;

        private float _currentLengtHpercentage;
        private Coroutine _moveCoroutine;

        public event Action<float> CurrentLengtHpercentageIncreased;
        public event Action CirclePassed;
        
        public void Initialize(SplineContainer spline)
        {
            _spline = spline;
        }
        
        public void StartMove()
        {
            _currentLengtHpercentage = 0f;

            this.StopCurrentCoroutine(ref _moveCoroutine);
            _moveCoroutine = StartCoroutine(Move());
        }

        public void StopMove()
        {
            this.StopCurrentCoroutine(ref _moveCoroutine);
        }

        public void IncreaseSpeed()
        {
            _speed *= _speedModifier; 
        }

        private IEnumerator Move()
        {
            while (_currentLengtHpercentage < MaxSplineLengtHpercentage)
            {
                _currentLengtHpercentage += _speed * Time.deltaTime;
                
                CurrentLengtHpercentageIncreased?.Invoke(_currentLengtHpercentage);
                
                transform.position = _spline.EvaluatePosition(_currentLengtHpercentage);

                yield return null;
            }
            
            CirclePassed?.Invoke();
        }
    }
}