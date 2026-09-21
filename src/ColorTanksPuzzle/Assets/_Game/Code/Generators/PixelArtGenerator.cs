using System;
using System.Collections.Generic;
using _Game.Code.Configurations;
using _Game.Code.Data;
using _Game.Code.Pixels;
using UnityEngine;

namespace _Game.Code.Generators
{
    public class PixelArtGenerator : MonoBehaviour
    {
        private const float HalfSizeDivisor = 2f;
        
        [SerializeField] private Pixel _pixelPrefab;
        [SerializeField] private Texture2D _sourceTexture;
        [SerializeField] private int _width = 20;
        [SerializeField] private int _height = 20;
        [SerializeField] private Palette _palette;
        [SerializeField] private Transform _container;

        private readonly List<PixelData> _pixelData = new();

        public event Action<List<PixelData>> ArtGenerated;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            Texture2D resizedTexture = ResizeTexture();

            float xOffset = (_width - 1) / HalfSizeDivisor - _container.position.x;
            float zOffset = _height / HalfSizeDivisor - _container.position.z;

            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    float positionX = x - xOffset;
                    float positionZ = z - zOffset;
                    Vector3 position = new Vector3(positionX, _container.transform.position.y, positionZ);

                    Pixel pixel = Instantiate(_pixelPrefab, position, Quaternion.identity, _container);
                    Color originalColor = resizedTexture.GetPixel(x, z);
                    Color pixelColor = ApplyColor(pixel, originalColor);
                    int depth = DefineDepth(x, z);
                    
                    _pixelData.Add(new PixelData(pixelColor, depth));
                }
            }

            ArtGenerated?.Invoke(_pixelData);
        }

        private Texture2D ResizeTexture()
        {
            Texture2D texture = new Texture2D(_width, _height);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    float u = (x + 0.5f) / _width;
                    float v = (y + 0.5f) / _height;

                    Color color = _sourceTexture.GetPixelBilinear(u, v);

                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();

            return texture;
        }

        private Color ApplyColor(Pixel pixel, Color originalColor)
        {
            Renderer rendererComponent = pixel.GetComponent<Renderer>();

            if (rendererComponent == null)
                throw new ArgumentNullException(nameof(rendererComponent));

            Color paletteColor = FindClosestColorInPalette(originalColor);

            Material newMaterial = new Material(rendererComponent.sharedMaterial)
            {
                color = paletteColor
            };

            rendererComponent.sharedMaterial = newMaterial;

            return paletteColor;
        }

        private Color FindClosestColorInPalette(Color originalColor)
        {
            int bestPaletteColorIndex = 0;
            float paletteColorMagnitude = float.MaxValue;

            for (int i = 0; i < _palette.Colors.Count; i++)
            {
                float originalColorMagnitude = Mathf.Pow(originalColor.r - _palette.Colors[i].r, 2)
                                               + Mathf.Pow(originalColor.g - _palette.Colors[i].g, 2)
                                               + Mathf.Pow(originalColor.b - _palette.Colors[i].b, 2);

                if (originalColorMagnitude < paletteColorMagnitude)
                {
                    paletteColorMagnitude = originalColorMagnitude;
                    bestPaletteColorIndex = i;
                }
            }

            return _palette.Colors[bestPaletteColorIndex];
        }

        private int DefineDepth(int x, int z)
        {
            bool isFirst = x is <= 2 or >= 18 && z is <= 2 or >= 18;
            bool isSecond= x is > 2 and <= 6 or < 18 and >= 12 && z is > 2 and <= 6 or < 18 and >= 12;
            bool isThird = x is > 6 and <= 11 && z is > 6 and <= 11;

            if (isFirst)
                return 0;
            else if (isSecond)
                return 1;
            else if (isThird)
                return 2;
            else
                return 2;
        }
    }
}