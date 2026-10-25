using System.Collections;
using _Game.Code.Extensions;
using SplineMesh;
using Unity.Mathematics;
using UnityEngine;

namespace _Game.Code.Tanks
{
    public class TankRotator : MonoBehaviour
    {
        [SerializeField] private Transform _hull;
        [SerializeField] private Transform _turret;

        private Spline _spline;

        private float _currentLengthPercentage;
        private bool _isRotating;
        private Coroutine _rotateCoroutine;

        public void Initialize(Spline spline) => 
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
                _currentLengthPercentage = Mathf.Min(_currentLengthPercentage, _spline.Length);
                float3 localTangent = _spline.GetSampleAtDistance(_currentLengthPercentage).tangent;
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
                float3 localUpVector = _spline.GetSampleAtDistance(_currentLengthPercentage).up;
                Vector3 worldUpDirection = _spline.transform.TransformDirection(localUpVector).normalized;
                Vector3 worldLeftDirection = Vector3.Cross(forwardWorldDirection, worldUpDirection);

                _turret.rotation = Quaternion.LookRotation(worldLeftDirection, _spline.transform.up);
            }
        }
    }
}