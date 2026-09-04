using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Code
{
    public class WaitingArea : MonoBehaviour
    {
        [SerializeField] private List<WaitingAreaCell> _waitingAreaCells;

        private int _freeCellsCount;
        
        public event Action Overflowed;

        private void Awake()
        {
            _freeCellsCount = _waitingAreaCells.Count;
        }

        public void Add(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));
            
            if (_freeCellsCount <= 0)
            {
                Overflowed?.Invoke();
            }
            else
            {
                _freeCellsCount--;
                WaitingAreaCell waitingAreaCell = GetFreeCell();
                waitingAreaCell.TakeOver(tank);
                tank.transform.position = waitingAreaCell.PointPosition;
            }
        }

        public void Remove(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));
            
            if (_freeCellsCount >= _waitingAreaCells.Count)
                throw new IndexOutOfRangeException();

            _freeCellsCount++;
            WaitingAreaCell waitingAreaCell = GetTakenCell(tank);
            waitingAreaCell.Release();
            tank.StartMove();
        }
        
        private WaitingAreaCell GetFreeCell()
        {
            WaitingAreaCell waitingAreaCell = _waitingAreaCells.FirstOrDefault(cell => cell.IsFree);

            if (waitingAreaCell is null)
                throw new ArgumentNullException(nameof(waitingAreaCell));
            
            return waitingAreaCell;
        }

        private WaitingAreaCell GetTakenCell(Tank tank)
        {
            WaitingAreaCell waitingAreaCell = _waitingAreaCells.FirstOrDefault(cell => cell.TakenTank.Equals(tank));

            if (waitingAreaCell is null)
                throw new ArgumentNullException(nameof(waitingAreaCell));
            
            return waitingAreaCell;
        }
    }
}