using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Data;
using _Game.Code.Generators.Tanks;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Tanks;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;

namespace _Game.Code.Spawners
{
    public class TankSpawner : MonoBehaviour, IDisposable
    {
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private SpawningLane[] _lanes;

        private readonly List<Tank> _spawnedTanks = new();

        private TanksDataGenerator _tanksDataGenerator;
        private WaitingArea _waitingArea;
        
        private Tank _tankPrefab;

        public int LanesCount => _lanes.Length;
        
        public event Action<List<Tank>> TanksSpawned;

        [Inject]
        public void Construct(TanksDataGenerator tanksDataGenerator, WaitingArea waitingArea, LoadService loadService)
        {
            _tanksDataGenerator = tanksDataGenerator;
            _waitingArea = waitingArea;
            
            _tankPrefab = loadService.LoadTank();
            
            _tanksDataGenerator.DataGenerated += OnDataGenerated;
        }

        public void Dispose() => 
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