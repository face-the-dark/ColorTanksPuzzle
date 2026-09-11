using UnityEngine;

namespace _Game.Code.Data
{
    public class PixelData
    {
        public PixelData(float x, float z, Color color)
        {
            X = x;
            Z = z;
            Color = color;
        }

        public float X { get; }
        public float Z { get; }
        public Color Color { get; }
    }
}