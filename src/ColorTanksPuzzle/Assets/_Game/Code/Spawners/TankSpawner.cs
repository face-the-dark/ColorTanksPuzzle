using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Generators.Data;
using _Game.Code.Generators.Tanks;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Spawners.Lines;
using _Game.Code.TankComponents;
using _Game.Code.WaitingAreaComponents;
using SplineMesh;
using VContainer;

namespace _Game.Code.Spawners
{
    public class TankSpawner : IDisposable
    {
        private readonly IReadOnlyList<SpawnLine> _spawnLines;
        private readonly List<Tank> _spawnedTanks = new();
        private readonly TanksDataGenerator _tanksDataGenerator;
        private readonly GameFactory _gameFactory;

        public event Action<List<Tank>> TanksSpawned;

        [Inject]
        public TankSpawner
        (
            SpawnLines spawnLines,
            TanksDataGenerator tanksDataGenerator,
            WaitingArea waitingArea,
            IAssetLoadService assetLoadService,
            GameFactory gameFactory
        )
        {
            _spawnLines = spawnLines.Lines;
            _tanksDataGenerator = tanksDataGenerator;
            _gameFactory = gameFactory;

            _tanksDataGenerator.DataGenerated += SpawnTanks;
        }

        public void Dispose() =>
            _tanksDataGenerator.DataGenerated -= SpawnTanks;

        private void SpawnTanks(List<TankData> tanksData)
        {
            for (int i = 0; i < _spawnLines.Count; i++)
            {
                int laneIndex = i;
                IEnumerable<TankData> tanksDataByLineIndex = tanksData.Where(x => x.LaneIndex == laneIndex);

                foreach (TankData tankData in tanksDataByLineIndex)
                {
                    SpawnLine spawnLine = _spawnLines[i];

                    Spawn(tankData, spawnLine);
                }
            }

            TanksSpawned?.Invoke(_spawnedTanks);

            foreach (SpawnLine spawnLine in _spawnLines)
                spawnLine.UnblockFirstTank();
        }

        private void Spawn(TankData tankData, SpawnLine spawnLine)
        {
            Tank tank = _gameFactory.CreateTank(spawnLine.transform);
            tank.Initialize(tankData);

            spawnLine.Add(tank);
            _spawnedTanks.Add(tank);
        }
    }
}