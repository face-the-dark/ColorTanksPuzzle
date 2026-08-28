using System;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class TankMovement : MonoBehaviour
    {
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private float _speed = 0.2f;

        private float _lengthPercentage;

        public event Action CirclePassed;
        
        private void Update()
        {
            _lengthPercentage += _speed * Time.deltaTime;

            transform.position  = _spline.EvaluatePosition(_lengthPercentage);

            if (_lengthPercentage >= 1) 
                CirclePassed?.Invoke();
        }
    }
}
