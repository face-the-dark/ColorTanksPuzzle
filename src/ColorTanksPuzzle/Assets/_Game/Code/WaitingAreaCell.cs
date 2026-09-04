using System;
using UnityEngine;

namespace _Game.Code
{
    public class WaitingAreaCell : MonoBehaviour
    {
        [SerializeField] private Transform _point;

        private bool _isFree = true;

        public Tank TakenTank { get; private set; }
        public Vector3 PointPosition => _point.position;
        public bool IsFree => _isFree;

        public void TakeOver(Tank tank)
        {
            if (_isFree == false)
                throw new Exception(nameof(_isFree));

            _isFree = false;
            TakenTank = tank;
        }

        public void Release()
        {
            if (_isFree)
                throw new Exception(nameof(_isFree));

            _isFree = true;
            TakenTank = null;
        }
    }
}