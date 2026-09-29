using System;
using _Game.Code.Pixels;
using _Game.Code.Providers;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Players
{
    public class Selector : IDisposable
    {
        private readonly Camera _camera;
        private readonly InputReader _inputReader;
        private readonly TankSettingsProvider _tankSettingsProvider;

        public event Action<Tank> TankSelected;
        public event Action<Pixel> PixelSelected;

        [Inject]
        public Selector(Camera camera, InputReader inputReader, TankSettingsProvider tankSettingsProvider)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));

            _tankSettingsProvider =
                tankSettingsProvider ?? throw new ArgumentNullException(nameof(tankSettingsProvider));

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
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tankSettingsProvider.TankSettings.TankLayer))
                if (hit.collider.TryGetComponent(out Tank tank) && tank.IsMoving == false)
                    TankSelected?.Invoke(tank);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                if (hit.collider.TryGetComponent(out Pixel pixel))
                    PixelSelected?.Invoke(pixel);
        }
    }
}