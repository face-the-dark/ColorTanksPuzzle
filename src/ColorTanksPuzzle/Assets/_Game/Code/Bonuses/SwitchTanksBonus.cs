using System;
using _Game.Code.Cameras;
using _Game.Code.Pixels;
using _Game.Code.Players;
using _Game.Code.Spawners.Lines;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class SwitchTanksBonus : IBonus
    {
        private readonly Selector _selector;
        private readonly CameraMover _cameraMover;
        private readonly SpawnLines _spawnLines;

        private bool _isActivated;
        private bool _isBlocked;

        [Inject]
        public SwitchTanksBonus(Selector selector, CameraMover cameraMover, SpawnLines spawnLines)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _cameraMover = cameraMover ?? throw new ArgumentNullException(nameof(cameraMover));
            _spawnLines = spawnLines ?? throw new ArgumentNullException(nameof(spawnLines));
        }

        public event Action<IBonus> Activated;

        public Bonus Bonus => Bonus.SwitchTanks;

        public bool CanActivate => _isActivated == false;

        public void Activate()
        {
            if (_isActivated == false)
            {
                _selector.PixelSelected += SwitchTanks;
                
                _cameraMover.ZoomToArt();
                
                _isActivated = true;
            }
        }

        private void SwitchTanks(Pixel pixel)
        {
            if (_isActivated)
            {
                _selector.PixelSelected -= SwitchTanks;
                
                _spawnLines.SwitchTanks(pixel.Color);
                _cameraMover.ZoomToStart();
            
                _isActivated = false;
            
                Activated?.Invoke(this);
            }
        }
    }
}