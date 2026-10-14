using System;
using _Game.Code.Coins;
using _Game.Code.Infrastructure.Assets;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace _Game.Code.Spawners
{
    public class CoinSpawner
    {
        private readonly Camera _camera;
        private readonly RectTransform _coinUiTarget;
        private readonly Coin _coinPrefab;

        [Inject]
        public CoinSpawner(AssetLoadService assetLoadService, Camera camera, RectTransform coinUiTarget)
        {
            if (assetLoadService == null)
                throw new ArgumentNullException(nameof(assetLoadService));

            _camera = camera;
            _coinUiTarget = coinUiTarget;

            _coinPrefab = assetLoadService.LoadCoinPrefab();
        }

        public void Spawn(Vector3 at)
        {
            Coin coin = Object.Instantiate(_coinPrefab, at, Quaternion.identity);
            coin.Construct(_camera, _coinUiTarget);
        }
    }
}