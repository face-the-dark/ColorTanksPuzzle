using System;
using _Game.Code.Bullets;
using _Game.Code.Infrastructure.Assets;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure
{
    public class GameFactory
    {
        private readonly IObjectResolver _container;
        private readonly IAssetLoadService _assetLoadService;

        [Inject]
        public GameFactory(IObjectResolver container, IAssetLoadService assetLoadService)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _assetLoadService = assetLoadService ?? throw new ArgumentNullException(nameof(assetLoadService));
        }

        public Bullet CreateBullet(Vector3 position)
        {
            Bullet bulletPrefab = _assetLoadService.LoadBulletPrefab();
            
            return _container.Instantiate(bulletPrefab, position, Quaternion.identity);
        }
    }
}