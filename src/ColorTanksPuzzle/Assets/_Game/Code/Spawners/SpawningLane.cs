using System.Collections.Generic;
using _Game.Code.TankComponents;
using DG.Tweening;
using UnityEngine;

namespace _Game.Code.Spawners
{
    public class SpawningLane : MonoBehaviour
    {
        private const float MoveDuration = 0.5f;
        
        [SerializeField] private float _positionOffsetZ = 5f;

        private readonly List<Tank> _tanks = new();

        private float _currentPositionOffsetZ;
        private Sequence _moveSequence;

        public void Add(Tank tank)
        {
            _tanks.Add(tank);

            tank.transform.position = new Vector3
            (
                transform.position.x,
                transform.position.y,
                transform.position.z - _currentPositionOffsetZ
            );

            _currentPositionOffsetZ += _positionOffsetZ;
        }

        public void Remove(Tank tank)
        {
            if (_tanks.Contains(tank) && tank.IsBlocked == false)
            {
                MoveTanksInLane(tank);
                
                _tanks.Remove(tank);
            }
        }

        public void UnblockFirstTank()
        {
            if (_tanks.Count > 0)
                _tanks[0].Unblock();
        }

        private void MoveTanksInLane(Tank tank)
        {
            int tankIndex = _tanks.IndexOf(tank);

            _moveSequence = DOTween.Sequence();
            
            for (int i = _tanks.Count - 1; i > tankIndex; i--)
            {
                Transform currentTank = _tanks[i].transform;
                Vector3 nextTankPosition = _tanks[i - 1].transform.position;

                _moveSequence.Join(currentTank.DOMove(nextTankPosition, MoveDuration).SetEase(Ease.Linear));
            }

            _moveSequence.onComplete += OnMoveSequenceComplete;
        }

        private void OnMoveSequenceComplete()
        {
            _moveSequence.onComplete -= OnMoveSequenceComplete;
            
            UnblockFirstTank();
        }
    }
}