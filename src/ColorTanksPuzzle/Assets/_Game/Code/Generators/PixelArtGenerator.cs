using System;
using System.Collections.Generic;
using _Game.Code.Configurations;
using _Game.Code.Configurations.Levels;
using _Game.Code.Configurations.Palettes;
using _Game.Code.Data;
using _Game.Code.Infrastructure;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Pixels;
using _Game.Code.Providers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Code.Generators
{
    public class PixelArtGenerator
    {
        private const int SquareLength = 20;

        private const int Offset = 1;
        private const float Divisor = 2f;

        private readonly Pixel _pixelPrefab;
        private readonly Palette _palette;
        private readonly Transform _container;
        private readonly LevelConfiguration _levelConfiguration;
        private readonly List<PixelData> _pixelData = new();

        public event Action<List<PixelData>> ArtGenerated;

        public PixelArtGenerator
        (
            Transform pixelArtContainer,
            LevelConfigurationProvider levelConfigurationProvider,
            LoadService loadService
        )
        {
            _levelConfiguration = levelConfigurationProvider.LevelConfiguration
                                  ?? throw new ArgumentNullException(nameof(levelConfigurationProvider));

            _container = pixelArtContainer ?? throw new ArgumentNullException(nameof(pixelArtContainer));

            _pixelPrefab = loadService.LoadPixel() ?? throw new ArgumentNullException(nameof(loadService));
            _palette = loadService.LoadPalette() ?? throw new ArgumentNullException(nameof(loadService));
        }

        public void Generate()
        {
            Texture2D resizedTexture = ResizeTexture();

            float xOffset = (SquareLength - Offset) / Divisor - _container.position.x;
            float zOffset = (SquareLength - Offset) / Divisor - _container.position.z;

            for (int z = 0; z < SquareLength; z++)
            {
                for (int x = 0; x < SquareLength; x++)
                {
                    float positionX = x - xOffset;
                    float positionZ = z - zOffset;
                    Vector3 position = new Vector3(positionX, _container.transform.position.y, positionZ);

                    Pixel pixel = Object.Instantiate(_pixelPrefab, position, Quaternion.identity, _container);
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
            Texture2D texture = new Texture2D(SquareLength, SquareLength);

            for (int y = 0; y < SquareLength; y++)
            {
                for (int x = 0; x < SquareLength; x++)
                {
                    float u = (x + 0.5f) / SquareLength;
                    float v = (y + 0.5f) / SquareLength;

                    Color color = _levelConfiguration.Picture.GetPixelBilinear(u, v);

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
            int distanceFromCenterX = Mathf.Abs(x - 9);
            int distanceFromCenterZ = Mathf.Abs(z - 9);

            int distanceFromCenter = Mathf.Max(
                distanceFromCenterX,
                distanceFromCenterZ
            );

            if (distanceFromCenter >= 7)
                return 0;

            if (distanceFromCenter >= 3)
                return 1;

            return 2;
        }
    }
}