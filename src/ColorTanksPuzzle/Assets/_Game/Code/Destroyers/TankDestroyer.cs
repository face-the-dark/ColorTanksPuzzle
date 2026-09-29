using System;
using System.Collections.Generic;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Destroyers
{
    public class TankDestroyer : IDisposable
    {
        private readonly TankSpawner _tankSpawner;
        
        private List<Tank> _tanks;

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
        }
        
        public void DestroyAllByColor(Color color)
        {
            _tanks.FindAll(x => x.Color == color).ForEach(Destroy);
        }
        
        private void OnTankSpawned(List<Tank> tanks)
        {
            _tanks = tanks;

            foreach (Tank tank in tanks) 
                tank.Died += OnDied;
        }

        private void OnDied(Tank tank)
        {
            tank.Died -= OnDied;
            
            Destroy(tank);
        }
    }
}