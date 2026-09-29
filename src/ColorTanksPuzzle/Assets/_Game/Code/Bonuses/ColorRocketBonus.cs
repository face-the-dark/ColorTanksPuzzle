using System;
using _Game.Code.Cameras;
using _Game.Code.ColorRocketComponents;
using _Game.Code.Destroyers;
using _Game.Code.Infrastructure.Assets;
using _Game.Code.Pixels;
using _Game.Code.Players;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace _Game.Code.Bonuses
{
    public class ColorRocketBonus : IDisposable, IBonus
    {
        private readonly PixelDestroyer _pixelDestroyer;
        private readonly TankDestroyer _tankDestroyer;
        private readonly CameraMover _cameraMover;
        private readonly Selector _selector;
        private readonly ColorRocket _colorRocketPrefab;

        private bool _isActivated;

        [Inject]
        public ColorRocketBonus
        (
            PixelDestroyer pixelDestroyer,
            TankDestroyer tankDestroyer,
            CameraMover cameraMover,
            Selector selector,
            LoadService loadService
        )
        {
            _pixelDestroyer = pixelDestroyer ?? throw new ArgumentNullException(nameof(pixelDestroyer));
            _tankDestroyer = tankDestroyer ?? throw new ArgumentNullException(nameof(tankDestroyer));
            _cameraMover = cameraMover ?? throw new ArgumentNullException(nameof(cameraMover));
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));

            _colorRocketPrefab = loadService.LoadColorRocketPrefab();

            _selector.PixelSelected += LaunchRocket;
        }

        public event Action<IBonus> Activated;

        public Bonus Bonus => Bonus.ColorRocket;

        public void Dispose() => 
            _selector.PixelSelected -= LaunchRocket;

        public void Activate()
        {
            if (_isActivated == false)
            {
                _cameraMover.ZoomToArt();
                _isActivated = true;
            }
        }

        private void LaunchRocket(Pixel pixel)
        {
            if (_isActivated)
            {
                ColorRocket colorRocket = Object.Instantiate
                (
                    _colorRocketPrefab,
                    _colorRocketPrefab.transform.position,
                    _colorRocketPrefab.transform.rotation
                );
                
                colorRocket.RocketArrived += Explode;

                Color pixelColor = pixel.Color;
                colorRocket.Initialize(pixelColor);
            }
        }

        private void Explode(ColorRocket colorRocket)
        {
            colorRocket.RocketArrived -= Explode;

            Color rocketColor = colorRocket.Color;

            _pixelDestroyer.DestroyAllByColor(rocketColor);
            _tankDestroyer.DestroyAllByColor(rocketColor);

            _isActivated = false;

            _cameraMover.ZoomToStart();
            
            Object.Destroy(colorRocket.gameObject);

            Activated?.Invoke(this);
        }
    }
}