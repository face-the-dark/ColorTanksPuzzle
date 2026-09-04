using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class Tank : MonoBehaviour
    {
        private const float MaxSplineLengthPercentage = 1f;

        [SerializeField] private float _speed = 0.2f;
        [SerializeField] private Transform _hull;
        [SerializeField] private Transform _turret;

        private SplineContainer _spline;

        private float _currentLengthPercentage;
        private Coroutine _moveCoroutine;
        private Coroutine _rotateCoroutine;

        public event Action<Tank> CirclePassed;

        public void Initialize(SplineContainer spline)
        {
            _spline = spline;
        }

        public void StartMove()
        {
            _currentLengthPercentage = 0f;

            StopCurrentCoroutine(ref _moveCoroutine);
            StopCurrentCoroutine(ref _rotateCoroutine);

            _moveCoroutine = StartCoroutine(Move());
            _rotateCoroutine = StartCoroutine(Rotate());
        }

        private void StopCurrentCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator Move()
        {
            while (_currentLengthPercentage < MaxSplineLengthPercentage)
            {
                _currentLengthPercentage += _speed * Time.deltaTime;
                transform.position = _spline.EvaluatePosition(_currentLengthPercentage);

                yield return null;
            }

            StopCurrentCoroutine(ref _moveCoroutine);
            
            CirclePassed?.Invoke(this);
        }

        private IEnumerator Rotate()
        {
            while (_currentLengthPercentage < MaxSplineLengthPercentage)
            {
                float3 localTangent = _spline.EvaluateTangent(_currentLengthPercentage);
                Vector3 forwardWorldDirection = _spline.transform.TransformDirection(localTangent).normalized;

                RotateHull(forwardWorldDirection);
                RotateTurret(forwardWorldDirection);

                yield return null;
            }

            StopCurrentCoroutine(ref _rotateCoroutine);

            _hull.rotation = Quaternion.LookRotation(Vector3.forward, _spline.transform.up);
            _turret.rotation = Quaternion.LookRotation(Vector3.forward, _spline.transform.up);
        }

        private void RotateHull(Vector3 forwardWorldDirection)
        {
            _hull.rotation = Quaternion.LookRotation(forwardWorldDirection, _spline.transform.up);
        }

        private void RotateTurret(Vector3 forwardWorldDirection)
        {
            float3 localUpVector = _spline.EvaluateUpVector(_currentLengthPercentage);
            Vector3 worldUpDirection = _spline.transform.TransformDirection(localUpVector).normalized;
            Vector3 worldLeftDirection = Vector3.Cross(forwardWorldDirection, worldUpDirection);
            
            _turret.rotation = Quaternion.LookRotation(worldLeftDirection, _spline.transform.up);
        }
    }
}