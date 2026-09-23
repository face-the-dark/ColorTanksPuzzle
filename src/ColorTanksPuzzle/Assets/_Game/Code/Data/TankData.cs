using UnityEngine;

namespace _Game.Code.Data
{
    public class TankData
    {
        public TankData(Color color, int hp, int depth)
        {
            Color = color;
            Hp = hp;
            Depth = depth;
        }

        public Color Color { get; }
        public int Hp { get; }
        public int Depth { get; }
        public int LaneIndex { get; set; }
        
        
    }
}