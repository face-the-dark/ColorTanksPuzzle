using UnityEngine;

namespace _Game.Code.Data
{
    public class TankData
    {
        public TankData(Color color, int hp, int laneIndex, int depth)
        {
            Color = color;
            Hp = hp;
            LaneIndex = laneIndex;
            Depth = depth;
        }

        public Color Color { get; }
        public int Hp { get; }
        public int LaneIndex { get; }
        public int Depth { get; }
    }
}