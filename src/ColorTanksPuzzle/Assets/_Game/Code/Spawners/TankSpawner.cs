using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Data;
using _Game.Code.Generators;
using _Game.Code.Tanks;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Spawners
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Tank _tankPrefab;
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private TanksDataGenerator _tanksDataGenerator;
        [SerializeField] private WaitingArea _waitingArea;
        [SerializeField] private SpawningLane[] _lanes;
        
        private readonly List<Tank> _spawnedTanks = new();
        
        public event Action<List<Tank>> TanksSpawned; 

        private void OnEnable() => 
            _tanksDataGenerator.DataGenerated += OnDataGenerated;

        private void OnDisable() => 
            _tanksDataGenerator.DataGenerated -= OnDataGenerated;

        private void OnDataGenerated(List<TankData> tanksData) => 
            SpawnTanks(tanksData);

        private void SpawnTanks(List<TankData> tanksData)
        {
            for (int i = 0; i < _lanes.Length; i++)
            {
                IEnumerable<TankData> tankDatas = tanksData
                    .Where(x => x.LaneIndex == i);
                
                foreach (TankData tankData in tankDatas)
                {
                    SpawningLane lane = _lanes[i];

                    Spawn(tankData, lane);
                }
            }

            TanksSpawned?.Invoke(_spawnedTanks);

            foreach (SpawningLane spawningLane in _lanes) 
                spawningLane.UnblockFirstTank();
        }

        private void Spawn(TankData tankData, SpawningLane lane)
        {
            Tank tank = Instantiate
            (
                _tankPrefab,
                Vector3.zero,
                Quaternion.identity,
                lane.transform
            );
            
            lane.Add(tank);
            _spawnedTanks.Add(tank);
            
            tank.Initialize(_spline, tankData, _waitingArea, lane);
        }
    }
}