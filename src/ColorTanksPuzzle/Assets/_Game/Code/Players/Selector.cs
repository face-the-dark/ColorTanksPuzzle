using System;
using _Game.Code.Pixels;
using _Game.Code.Providers;
using _Game.Code.TankComponents;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class Selector : IDisposable
    {
        private readonly Camera _camera;
        private readonly InputReader _inputReader;
        private readonly TankConfigurationProvider _tankConfigurationProvider;

        public event Action<Tank> TankSelected;
        public event Action<Pixel> PixelSelected;

        [Inject]
        public Selector(Camera camera, InputReader inputReader, TankConfigurationProvider tankConfigurationProvider)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));

            _tankConfigurationProvider =
                tankConfigurationProvider ?? throw new ArgumentNullException(nameof(tankConfigurationProvider));

            _inputReader.Clicked += TrySelectUnmovingTank;
        }

        public void Dispose()
        {
            _inputReader.Clicked -= TrySelectUnmovingTank;
        }

        private void TrySelectUnmovingTank(Vector2 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);
            
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tankConfigurationProvider.TankConfiguration.TankLayer))
                if (hit.collider.TryGetComponent(out Tank tank) && tank.IsMoving == false)
                    TankSelected?.Invoke(tank);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                if (hit.collider.TryGetComponent(out Pixel pixel))
                    PixelSelected?.Invoke(pixel);
        }
    }
}