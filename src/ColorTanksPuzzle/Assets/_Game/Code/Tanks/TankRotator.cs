using System.Collections;
using _Game.Code.Extensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class TankRotator : MonoBehaviour
    {
        [SerializeField] private Transform _hull;
        [SerializeField] private Transform _turret;

        private SplineContainer _spline;

        private float _currentLengthPercentage;
        private bool _isRotating;
        private Coroutine _rotateCoroutine;

        public void Initialize(SplineContainer spline) => 
            _spline = spline;

        public void StartRotate()
        {
            StopRotate();
            _isRotating = true;
            _rotateCoroutine = StartCoroutine(Rotate());
        }

        public void StopRotate()
        {
            _isRotating = false;
            this.StopCurrentCoroutine(ref _rotateCoroutine);

            ResetRotation();
        }

        public void UpdateCurrentLengthPercentage(float currentLengthPercentage) => 
            _currentLengthPercentage = currentLengthPercentage;

        private IEnumerator Rotate()
        {
            while (_isRotating)
            {
                float3 localTangent = _spline.EvaluateTangent(_currentLengthPercentage);
                Vector3 forwardWorldDirection = _spline.transform.TransformDirection(localTangent).normalized;

                RotateHull(forwardWorldDirection);
                RotateTurret(forwardWorldDirection);

                yield return null;
            }
        }

        private void ResetRotation()
        {
            _hull.rotation = Quaternion.LookRotation(Vector3.forward, _spline.transform.up);
            _turret.rotation = Quaternion.LookRotation(Vector3.forward, _spline.transform.up);
        }

        private void RotateHull(Vector3 forwardWorldDirection)
        {
            if (forwardWorldDirection != Vector3.zero)
                _hull.rotation = Quaternion.LookRotation(forwardWorldDirection, _spline.transform.up);
        }

        private void RotateTurret(Vector3 forwardWorldDirection)
        {
            if (forwardWorldDirection != Vector3.zero)
            {
                float3 localUpVector = _spline.EvaluateUpVector(_currentLengthPercentage);
                Vector3 worldUpDirection = _spline.transform.TransformDirection(localUpVector).normalized;
                Vector3 worldLeftDirection = Vector3.Cross(forwardWorldDirection, worldUpDirection);

                _turret.rotation = Quaternion.LookRotation(worldLeftDirection, _spline.transform.up);
            }
        }
    }
}