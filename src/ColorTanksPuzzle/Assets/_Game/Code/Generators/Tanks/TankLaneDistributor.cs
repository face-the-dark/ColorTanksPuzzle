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

        public List<TankData> Distribute(List<TankData> tanks)
        {
            if  (tanks == null || tanks.Count == 0)
                throw new ArgumentNullException(nameof(tanks));
            
            List<TankData> result = new List<TankData>(tanks.Count);

            for (int i = 0; i < tanks.Count; i++)
            {
                TankData tank = tanks[i];

                int laneIndex = i % _laneCount;

                result.Add(
                    new TankData(
                        tank.Color,
                        tank.Hp,
                        tank.Depth,
                        laneIndex
                    )
                );
            }

            return result;
        }
    }
}
