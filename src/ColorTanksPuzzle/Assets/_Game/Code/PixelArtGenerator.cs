using UnityEngine;

namespace _Game.Code
{
    public class PixelArtGenerator : MonoBehaviour
    {
        [SerializeField] private Pixel _pixelPrefab;
        [SerializeField] private Texture2D _sourceTexture;
        [SerializeField] private int _width = 20;
        [SerializeField] private int _height = 20;
        [SerializeField] private Palette _palette;
        [SerializeField] private Transform _container;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            Texture2D resizedTexture = ResizeTexture();

            float xOffset = (_width - 1) / 2f - _container.position.x;
            float zOffset = _height / 2f - _container.position.z;

            for (int z = 0; z < _height; z++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Vector3 position = new Vector3(x - xOffset, 1.5f, z - zOffset);
                    Pixel pixel = Instantiate(_pixelPrefab, position, Quaternion.identity, _container);
                    Color originalColor = resizedTexture.GetPixel(x, z);
                    ApplyColor(originalColor, pixel);
                }
            }
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

        private void ApplyColor(Color originalColor, Pixel pixel)
        {
            Color paletteColor = FindClosestColorInPalette(originalColor);
            Renderer rendererComponent = pixel.GetComponent<Renderer>();
            Material sharedMaterial = rendererComponent.sharedMaterial;
            Material newMaterial = new Material(sharedMaterial)
            {
                color = paletteColor
            };
            rendererComponent.sharedMaterial = newMaterial;
        }

        private Color FindClosestColorInPalette(Color originalColor)
        {
            int bestIndex = 0;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < _palette.Colors.Count; i++)
            {
                float magnitude = Mathf.Pow(originalColor.r - _palette.Colors[i].r, 2)
                                  + Mathf.Pow(originalColor.g - _palette.Colors[i].g, 2)
                                  + Mathf.Pow(originalColor.b - _palette.Colors[i].b, 2);

                if (magnitude < bestDistance)
                {
                    bestDistance = magnitude;
                    bestIndex = i;
                }
            }

            return _palette.Colors[bestIndex];
        }
    }
}