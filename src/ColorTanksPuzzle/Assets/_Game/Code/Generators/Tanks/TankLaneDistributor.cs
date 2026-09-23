using System;
using System.Collections.Generic;
using _Game.Code.Data;

namespace _Game.Code.Generators.Tanks
{
    public class TankLaneDistributor
    {
        private readonly int _laneCount;

        public TankLaneDistributor(int laneCount)
        {
            if (laneCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(laneCount));

            _laneCount = laneCount;
        }

        public void Distribute(List<TankData> tanks)
        {
            if  (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));

            for (int i = 0; i < tanks.Count; i++)
            {
                int laneIndex = i % _laneCount;
                tanks[i].LaneIndex = laneIndex;
            }
        }
    }
}
