using System.Collections.Generic;
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

        private void OnEnable() => 
            _tanksDataGenerator.DataGenerated += OnDataGenerated;

        private void OnDisable() => 
            _tanksDataGenerator.DataGenerated -= OnDataGenerated;

        private void OnDataGenerated(List<TankData> tanksData)
        {
            SpawnTanks(tanksData);
        }

        private void SpawnTanks(List<TankData> tanksData)
        {
            for (int i = 0; i < _lanes.Length; i++)
            {
                List<TankData> laneTanksData = tanksData.FindAll(x => x.LaneIndex == i);

                foreach (TankData tankData in laneTanksData)
                {
                    SpawningLane lane = _lanes[tankData.LaneIndex];

                    Spawn(tankData, lane);
                }
            }

            foreach (SpawningLane spawningLane in _lanes)
            {
                spawningLane.UnblockFirstTank();
            }
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

            tank.Initialize(_spline, tankData, _waitingArea, lane);
        }
    }
}