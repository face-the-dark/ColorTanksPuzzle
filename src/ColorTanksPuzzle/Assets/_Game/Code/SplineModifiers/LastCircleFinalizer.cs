using System;
using System.Collections.Generic;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using VContainer;

namespace _Game.Code.SplineModifiers
{
    public class LastCircleFinalizer : IDisposable
    {
        private readonly TankSpawner _tankSpawner;

        private List<Tank> _tanks = new();
        private bool _isActive;

        [Inject]
        public LastCircleFinalizer(TankSpawner tankSpawner)
        {
            _tankSpawner = tankSpawner;
            
            _tankSpawner.TanksSpawned += OnTanksSpawned;
        }

        public void Dispose() =>
            _tankSpawner.TanksSpawned -= OnTanksSpawned;

        private void OnTanksSpawned(List<Tank> tanks)
        {
            _tanks = new List<Tank>(tanks);

            foreach (Tank tank in _tanks)
                tank.Died += OnDied;
        }

        private void OnDied(Tank tank)
        {
            tank.Died -= OnDied;

            _tanks.Remove(tank);

            if (_isActive == false && _tanks.Count <= 5)
                Activate();
        }

        private void Activate()
        {
            _isActive = true;

            foreach (Tank tank in _tanks)
                tank.StartLoopMove();
        }
    }
}