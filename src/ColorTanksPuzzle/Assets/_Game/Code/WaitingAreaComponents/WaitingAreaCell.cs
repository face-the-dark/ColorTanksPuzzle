using System;
using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.WaitingAreaComponents
{
    public class WaitingAreaCell : MonoBehaviour
    {
        [SerializeField] private Transform _point;

        private bool _isFree = true;
        private Tank _takenTank;

        public bool IsFree => _isFree;
        public Tank TakenTank => _takenTank;

        public void TakeOver(Tank tank)
        {
            if (_isFree == false)
                throw new Exception(nameof(_isFree));

            _isFree = false;
            _takenTank = tank;
            
            tank.transform.position = _point.position;
        }

        public void Release()
        {
            if (_isFree)
                throw new Exception(nameof(_isFree));

            _isFree = true;
            _takenTank = null;
        }
    }
}