using System;
using System.Collections.Generic;

namespace _Game.Code
{
    public class WaitingArea
    {
        private const int CellCount = 5;
        
        private List<Tank> _tanks = new();
        
        public event Action<Tank> Added;
        public event Action<Tank> Removed;

        public bool Contains(Tank tank) => 
            _tanks.Contains(tank);
        
        public void Add(Tank tank)
        {
            if (tank == null)
                throw new ArgumentNullException(nameof(tank));
            
            if (_tanks.Count > CellCount)
                throw new IndexOutOfRangeException();
            
            _tanks.Add(tank);
            Added?.Invoke(tank);
        }

        public void Remove(Tank tank)
        {
            if (tank == null)
                throw new ArgumentNullException(nameof(tank));
            
            if (_tanks.Count < CellCount)
                throw new IndexOutOfRangeException();
            
            _tanks.Remove(tank);
            Removed?.Invoke(tank);
        }
    }
}