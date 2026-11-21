using System;
using System.Collections.Generic;
using _Game.Code.Generators.Data;
using _Game.Code.Spawners;
using _Game.Code.Spawners.Lines;
using VContainer;

namespace _Game.Code.Generators.Tanks
{
    public class TankLaneDistributor
    {
        private readonly int _laneCount;

        [Inject]
        public TankLaneDistributor(SpawnLines spawnLines)
        {
            if (spawnLines.LinesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(spawnLines.LinesCount));

            _laneCount = spawnLines.LinesCount;
        }

        public void Distribute(List<TankData> tanks)
        {
            if (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            for (int i = 0; i < tanks.Count; i++)
            {
                int laneIndex = i % _laneCount;
                tanks[i].LaneIndex = laneIndex;
            }
        }
    }
}