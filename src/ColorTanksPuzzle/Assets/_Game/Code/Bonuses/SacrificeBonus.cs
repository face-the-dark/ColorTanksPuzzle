using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Code.Cameras;
using _Game.Code.Destroyers;
using _Game.Code.Pixels;
using _Game.Code.Players;
using _Game.Code.Spawners;
using _Game.Code.Tanks;
using UnityEngine;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class SacrificeBonus : IDisposable, IBonus
    {
        private readonly Selector _selector;
        private readonly CameraMover _cameraMover;
        private readonly TankDestroyer _tankDestroyer;
        private readonly PixelDestroyer _pixelDestroyer;

        private bool _isActivated;
        
        [Inject]
        public SacrificeBonus
        (
            Selector selector,
            CameraMover cameraMover,
            TankDispatcher tankDispatcher,
            PixelArtSpawner pixelArtSpawner,
            TankDestroyer tankDestroyer,
            PixelDestroyer pixelDestroyer
        )
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _cameraMover = cameraMover ?? throw new ArgumentNullException(nameof(cameraMover));
            _tankDestroyer = tankDestroyer ?? throw new ArgumentNullException(nameof(tankDestroyer));
            _pixelDestroyer = pixelDestroyer ?? throw new ArgumentNullException(nameof(pixelDestroyer));

            _selector.TankSelected += Sacrifice;
        }
        
        public event Action<IBonus> Activated;
        
        public Bonus Bonus => Bonus.Sacrifice;

        public void Dispose()
        {
            _selector.TankSelected -= Sacrifice;
        }

        public void Activate()
        {
            if (_isActivated == false)
            {
                _cameraMover.ZoomToTanks();
                _isActivated = true;
            }
        }

        private void Sacrifice(Tank tank)
        {
            if (_isActivated)
            {
                Color tankColor = tank.Color;
                int tankHp = tank.Hp;

                if (_pixelDestroyer.TryGetPixelsByColor(tankColor, out List<Pixel> pixels))
                {
                    if (pixels.Count >= tankHp)
                    {
                        for (int i = tankHp; i > 0; i--)
                        {
                            Pixel pixel = pixels.First();
                            
                            pixels.Remove(pixel);
                            
                            _pixelDestroyer.Destroy(pixel);
                        }
                        
                        _tankDestroyer.Destroy(tank);
                    }
                }
                
                _cameraMover.ZoomToStart();
                _isActivated = false;
                
                Activated?.Invoke(this);
            }
        }
    }
}