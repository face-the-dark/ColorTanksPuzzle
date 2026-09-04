using System;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Tank _tankPrefab;
        [SerializeField] private SplineContainer _spline;

        public event Action<Tank> TankSpawned;
        
        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            Tank tank = Instantiate(_tankPrefab);
            tank.Initialize(_spline);
            TankSpawned?.Invoke(tank);
        }
    }
}