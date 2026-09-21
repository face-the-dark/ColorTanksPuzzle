using UnityEngine;

namespace _Game.Code.Data
{
    public class PixelData
    {
        public PixelData(Color color, int depth)
        {
            Color = color;
            Depth = depth;
        }

        public Color Color { get; }
        public int Depth { get; }
    }
}