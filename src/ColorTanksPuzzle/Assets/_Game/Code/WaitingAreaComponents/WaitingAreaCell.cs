using System;
using _Game.Code.TankComponents;
using DG.Tweening;
using UnityEngine;

namespace _Game.Code.WaitingAreaComponents
{
    public class WaitingAreaCell : MonoBehaviour
    {
        [SerializeField] private Transform _point;

        private Tank _takenTank;

        public bool IsFree => _takenTank == null;
        public Tank TakenTank => _takenTank;

        public event Action Spawned;
        
        public void Initialize()
        {
            Spawned?.Invoke();
        }

        public void TakeOver(Tank tank)
        {
            _takenTank = tank;
            
            tank.transform
                .DOJump(_point.position, 20f, 1, 0.5f)
                .SetEase(Ease.Linear);
        }

        public void Release()
        {
            _takenTank = null;
        }

        public void UpdateTakenTankPosition()
        {
            if (_takenTank != null) 
                _takenTank.transform.position = _point.position;
        }
    }
}