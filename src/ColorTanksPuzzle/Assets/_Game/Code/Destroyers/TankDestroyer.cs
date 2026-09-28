using System;
using System.Collections.Generic;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace _Game.Code.Destroyers
{
    public class TankDestroyer : IDisposable
    {
        private readonly TankSpawner _tankSpawner;
        
        private List<Tank> _tanks;
        
        public event Action<Tank> TankDestroyed;

        [Inject]
        public TankDestroyer(TankSpawner tankSpawner)
        {
            _tankSpawner = tankSpawner ?? throw new ArgumentNullException(nameof(tankSpawner));

            _tankSpawner.TanksSpawned += OnTankSpawned;
        }

        public void Dispose()
        {
            _tankSpawner.TanksSpawned -= OnTankSpawned;
        }

        public void Destroy(Tank tank)
        {
            _tanks.Remove(tank);
            
            tank.Unblock();
            tank.RemoveFromWaitingAreaOrLane();
            tank.Die();
            
            TankDestroyed?.Invoke(tank);
            
            Object.Destroy(tank.gameObject);
        }
        
        public void DestroyAllByColor(Color color)
        {
            _tanks.FindAll(x => x.Color == color).ForEach(tank =>
            {
                _tanks.Remove(tank);
            
                tank.Unblock();
                tank.RemoveFromWaitingAreaOrLane();
                tank.Die();
            
                TankDestroyed?.Invoke(tank);
            
                Object.Destroy(tank.gameObject);
            });
        }
        
        private void OnTankSpawned(List<Tank> tanks)
        {
            _tanks = tanks;
        }
    }
}