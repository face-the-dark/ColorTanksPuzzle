using System;
using System.Collections.Generic;
using _Game.Code.Generators.Data;
using _Game.Code.Spawners;
using VContainer;

namespace _Game.Code.Generators.Tanks
{
    public class TankLaneDistributor
    {
        private readonly int _laneCount;

        [Inject]
        public TankLaneDistributor(SpawningLanesContainer spawningLanesContainer)
        {
            if (spawningLanesContainer.LanesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(spawningLanesContainer.LanesCount));

            _laneCount = spawningLanesContainer.LanesCount;
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