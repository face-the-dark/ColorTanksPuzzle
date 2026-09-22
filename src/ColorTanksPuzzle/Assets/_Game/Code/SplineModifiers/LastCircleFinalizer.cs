using System.Collections.Generic;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using UnityEngine;

namespace _Game.Code.SplineModifiers
{
    public class LastCircleFinalizer : MonoBehaviour
    {
        [SerializeField] private TankSpawner _tankSpawner;

        private readonly List<Tank> _tanks = new();
        private bool _isActive;

        private void OnEnable() => 
            _tankSpawner.TanksSpawned += OnTanksSpawned;

        private void OnDisable() => 
            _tankSpawner.TanksSpawned -= OnTanksSpawned;

        private void OnTanksSpawned(List<Tank> tanks)
        {
            _tanks.AddRange(tanks);

            foreach (Tank tank in tanks) 
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
            if (_isActive == false)
            {
                _isActive = true;
            
                foreach (Tank tank in _tanks) 
                    tank.StartLoopMove();
            }
        }
    }
}