using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code
{
    public class WaitingAreaView : MonoBehaviour
    {
        [SerializeField] private List<WaitingAreaCell> _waitingAreaCells;

        private WaitingArea _waitingArea;
        
        private int _currentFreeCellIndex = 0;

        public event Action AreaOverflow;

        public void Initialize(WaitingArea waitingArea)
        {
            _waitingArea = waitingArea;
        }

        private void OnEnable() => 
            _waitingArea.Added += Put;

        private void OnDisable() => 
            _waitingArea.Added -= Put;

        private void Put(Tank tank)
        {
            if (tank == null)
                throw new ArgumentNullException(nameof(tank));

            if (_currentFreeCellIndex > 5) 
                AreaOverflow?.Invoke();
            
            WaitingAreaCell currentCell = _waitingAreaCells[_currentFreeCellIndex];
            _currentFreeCellIndex++;
            tank.transform.SetPositionAndRotation(currentCell.PointPosition, Quaternion.identity);
        }
    }
}