using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace _Game.Code
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Tank _tankPrefab;
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private PixelArtGenerator _pixelArtGenerator;
        [SerializeField] private Color _tempColor;

        public event Action<Tank> TankSpawned;

        private void OnEnable()
        {
            _pixelArtGenerator.ArtGenerated += OnArtGenerated;
        }

        private void OnDisable()
        {
            _pixelArtGenerator.ArtGenerated -= OnArtGenerated;
        }

        private void OnArtGenerated(List<Material> materials)
        {
           Spawn(materials.Select(x => x.color).First());
           
           StartCoroutine(Delay(materials));
        }

        private void Spawn(Color color)
        {
            Tank tank = Instantiate(_tankPrefab);
            tank.Initialize(_spline, color);
            TankSpawned?.Invoke(tank);
        }

        private IEnumerator Delay(List<Material> materials)
        {
            yield return new WaitForSeconds(1f);
            
            Spawn(materials.Select(x => x.color).First(x=> x == _tempColor));
        }
    }
}