using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.2f;

        private SplineContainer _spline;
        
        private float _currentLengthPercentage;
        private Coroutine _moveCoroutine;
        private bool _isMoving;
        
        public event Action<Tank> CirclePassed;

        public void Initialize(SplineContainer spline)
        {
            _spline = spline;
        }
        
        public void StartMove()
        {
            _currentLengthPercentage = 0f;
            StopMoveCoroutine();
            _isMoving = true;
            _moveCoroutine = StartCoroutine(Move());
        }

        private void StopMoveCoroutine()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
        }

        private IEnumerator Move()
        {
            while (_isMoving)
            {
                _currentLengthPercentage += _speed * Time.deltaTime;

                transform.position  = _spline.EvaluatePosition(_currentLengthPercentage);

                if (_currentLengthPercentage >= 1)
                {
                    StopMoveCoroutine();
                    _isMoving = false;
                    CirclePassed?.Invoke(this);
                }

                yield return null;
            }
        }
    }
}
