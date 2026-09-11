using UnityEngine;

namespace _Game.Code.Data
{
    public class TankData
    {
        public TankData(Color color, int hp, int laneIndex)
        {
            Color = color;
            Hp = hp;
            LaneIndex = laneIndex;
        }

        public Color Color { get; }
        public int Hp { get; }
        public int LaneIndex { get; }
    }
}