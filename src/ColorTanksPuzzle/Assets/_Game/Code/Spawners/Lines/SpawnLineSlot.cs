using System;
using _Game.Code.TankComponents;
using UnityEngine;

namespace _Game.Code.Spawners.Lines
{
    public class SpawnLineSlot
    {
        private readonly Vector3 _position;
        
        private Tank _tank;

        public Vector3 Position => _position;
        public Tank Tank => _tank;
        
        public SpawnLineSlot(Vector3 position, Tank tank)
        {
            _position = position;
            _tank = tank;
            _tank.transform.position = _position;
        }

        public void SetTank(Tank tank)
        {
            if (tank == null)
                throw new InvalidOperationException(nameof(tank));
            
            _tank = tank;
        }
        
        public void SetTankPosition(Tank tank)
        {
            if (tank == null)
                throw new InvalidOperationException(nameof(tank));
            
            _tank = tank;
            _tank.transform.position = _position;
        }

        public bool EqualsTank(Tank tank)
        {
            return _tank.Equals(tank);
        }
    }
}