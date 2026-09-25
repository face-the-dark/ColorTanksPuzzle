using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Cameras;
using _Game.Code.Generators;
using _Game.Code.Pixels;
using _Game.Code.Players;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class SacrificeBonus : IDisposable, IBonus
    {
        private readonly TankSelector _tankSelector;
        private readonly CameraMover _cameraMover;
        private readonly PixelArtGenerator _pixelArtGenerator;

        private bool _isActivated;
        private bool _isBlocked;

        private Dictionary<Color, List<Pixel>> _pixelsByColor;

        public event Action<IBonus> Activated;
        
        public Bonus Bonus => Bonus.Sacrifice;

        [Inject]
        public SacrificeBonus
        (
            TankSelector tankSelector,
            CameraMover cameraMover,
            TankDispatcher tankDispatcher,
            PixelArtGenerator pixelArtGenerator
        )
        {
            _tankSelector = tankSelector ?? throw new ArgumentNullException(nameof(tankSelector));
            _cameraMover = cameraMover ?? throw new ArgumentNullException(nameof(cameraMover));
            _pixelArtGenerator = pixelArtGenerator ?? throw new ArgumentNullException(nameof(pixelArtGenerator));

            _tankSelector.TankSelected += Sacrifice;
            _pixelArtGenerator.ArtGenerated += AddPixels;
        }

        private void AddPixels(List<Pixel> pixels)
        {
            _pixelsByColor = pixels
                .GroupBy(x => x.Color)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        public void Dispose()
        {
            _tankSelector.TankSelected -= Sacrifice;
            _pixelArtGenerator.ArtGenerated -= AddPixels;
        }

        public void Activate()
        {
            if (_isActivated == false)
            {
                _cameraMover.Unzoom();
                _isActivated = true;
            }
        }

        public void Block() =>
            _isBlocked = true;

        public void Unblock() =>
            _isBlocked = false;

        private void Sacrifice(Tank tank)
        {
            if (_isActivated)
            {
                Color tankColor = tank.Color;
                int tankHp = tank.Hp;

                if (_pixelsByColor.TryGetValue(tankColor, out List<Pixel> pixels))
                {
                    if (pixels.Count >= tankHp)
                    {
                        for (int i = tankHp; i > 0; i--)
                        {
                            Pixel pixel = pixels.First();
                            
                            pixels.Remove(pixel);
                            pixel.Die();
                        }

                        tank.Unblock();
                        tank.RemoveFromWaitingAreaOrLane();
                        tank.Die();
                    }
                }
                
                _cameraMover.Zoom();
                _isActivated = false;
                
                Activated?.Invoke(this);
            }
        }
    }
}