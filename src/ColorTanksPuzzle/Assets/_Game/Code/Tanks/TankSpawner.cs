using System.Collections.Generic;
using _Game.Code.Data;
using _Game.Code.Generators;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code.Tanks
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Tank _tankPrefab;
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private TanksDataGenerator _tanksDataGenerator;
        [SerializeField] private Transform[] _lanes;

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
            for (var i = 0; i < _lanes.Length; i++)
            {
                float zOffset = 0f;

                List<TankData> laneTanksData = tanksData.FindAll(x => x.LaneIndex == i);

                foreach (TankData tankData in laneTanksData)
                {
                    Transform lane = _lanes[tankData.LaneIndex];

                    Spawn(tankData, lane, zOffset);

                    zOffset += 8f;
                }
            }
        }

        private void Spawn(TankData tankData, Transform lane, float zOffset)
        {
            Vector3 position = new Vector3
            (
                lane.transform.position.x,
                lane.transform.position.y,
                lane.transform.position.z - zOffset
            );

            Tank tank = Instantiate
            (
                _tankPrefab,
                position,
                Quaternion.identity,
                lane
            );

            tank.Initialize(_spline, tankData);
        }
    }
}