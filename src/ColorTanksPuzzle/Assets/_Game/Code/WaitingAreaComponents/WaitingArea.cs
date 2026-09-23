using System;
using System.Collections.Generic;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using VContainer;

namespace _Game.Code.WaitingAreaComponents
{
    public class WaitingArea : IDisposable
    {
        private WaitingAreaCellSpawner _waitingAreaCellSpawner;
        
        private List<WaitingAreaCell> _waitingAreaCells;

        private int _freeCellsCount;
        
        public event Action Overflowed;

        [Inject]
        public void Construct(WaitingAreaCellSpawner waitingAreaCellSpawner)
        {
            _waitingAreaCellSpawner = waitingAreaCellSpawner;
            
            _waitingAreaCellSpawner.CellsSpawned += OnCellSpawned;
        }

        public void Dispose() => 
            _waitingAreaCellSpawner.CellsSpawned -= OnCellSpawned;

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
            }
        }

        public void Remove(Tank tank)
        {
            if (tank is null)
                throw new ArgumentNullException(nameof(tank));

            if (_freeCellsCount > _waitingAreaCells.Count)
                throw new IndexOutOfRangeException();

            _freeCellsCount++;
            WaitingAreaCell waitingAreaCell = GetTakenCell(tank);
            waitingAreaCell.Release();
        }

        private WaitingAreaCell GetFreeCell()
        {
            WaitingAreaCell waitingAreaCell = _waitingAreaCells.Find(cell => cell.IsFree);

            if (waitingAreaCell is null)
                throw new ArgumentNullException(nameof(waitingAreaCell));

            return waitingAreaCell;
        }

        private WaitingAreaCell GetTakenCell(Tank tank)
        {
            WaitingAreaCell waitingAreaCell = _waitingAreaCells.Find(cell => tank.Equals(cell.TakenTank));

            if (waitingAreaCell is null)
                throw new ArgumentNullException(nameof(waitingAreaCell));

            return waitingAreaCell;
        }

        private void OnCellSpawned(List<WaitingAreaCell> waitingAreaCells)
        {
            _waitingAreaCells = waitingAreaCells;
            
            _freeCellsCount = _waitingAreaCells.Count;
        }
    }
}