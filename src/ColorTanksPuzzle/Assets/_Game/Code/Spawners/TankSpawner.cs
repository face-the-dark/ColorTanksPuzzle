using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Destroyers;
using _Game.Code.Generators.Data;
using _Game.Code.Generators.Tanks;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Tanks;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;
using UnityEngine.Splines;
using VContainer;
using Object = UnityEngine.Object;

namespace _Game.Code.Spawners
{
    public class TankSpawner : IDisposable
    {
        private readonly SplineContainer _spline;
        private readonly IReadOnlyList<SpawningLane> _spawningLanes;
        private readonly List<Tank> _spawnedTanks = new();
        private readonly TanksDataGenerator _tanksDataGenerator;
        private readonly WaitingArea _waitingArea;
        private readonly PixelDestroyer _pixelDestroyer;
        private readonly Tank _tankPrefab;

        public event Action<List<Tank>> TanksSpawned;

        [Inject]
        public TankSpawner
        (
            SplineContainer spline,
            SpawningLanesContainer spawningLanesContainer,
            TanksDataGenerator tanksDataGenerator,
            WaitingArea waitingArea,
            LoadService loadService,
            PixelDestroyer pixelDestroyer
        )
        {
            _spline = spline;
            _spawningLanes = spawningLanesContainer.SpawningLanes;
            _tanksDataGenerator = tanksDataGenerator;
            _waitingArea = waitingArea;
            _pixelDestroyer = pixelDestroyer;

            _tankPrefab = loadService.LoadTank();

            _tanksDataGenerator.DataGenerated += OnDataGenerated;
        }

        public void Dispose() =>
            _tanksDataGenerator.DataGenerated -= OnDataGenerated;

        private void OnDataGenerated(List<TankData> tanksData) =>
            SpawnTanks(tanksData);

        private void SpawnTanks(List<TankData> tanksData)
        {
            for (int i = 0; i < _spawningLanes.Count; i++)
            {
                IEnumerable<TankData> tankDatas = tanksData
                    .Where(x => x.LaneIndex == i);

                foreach (TankData tankData in tankDatas)
                {
                    SpawningLane lane = _spawningLanes[i];

                    Spawn(tankData, lane);
                }
            }

            TanksSpawned?.Invoke(_spawnedTanks);

            foreach (SpawningLane spawningLane in _spawningLanes)
                spawningLane.UnblockFirstTank();
        }

        private void Spawn(TankData tankData, SpawningLane lane)
        {
            Tank tank = Object.Instantiate
            (
                _tankPrefab,
                Vector3.zero,
                Quaternion.identity,
                lane.transform
            );

            lane.Add(tank);
            _spawnedTanks.Add(tank);

            tank.Initialize(_spline, tankData, _waitingArea, lane, _pixelDestroyer);
        }
    }
}