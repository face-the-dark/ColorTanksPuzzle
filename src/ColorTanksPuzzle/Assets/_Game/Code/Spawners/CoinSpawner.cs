using System;
using _Game.Code.Coins;
using _Game.Code.Infrastructure;
using UnityEngine;
using VContainer;

namespace _Game.Code.Spawners
{
    public class CoinSpawner
    {
        private readonly IGameFactory _gameFactory;
        private readonly RectTransform _coinUiTarget;

        [Inject]
        public CoinSpawner(IGameFactory gameFactory, RectTransform coinUITarget)
        {
            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
            _coinUiTarget = coinUITarget ?? throw new ArgumentNullException(nameof(coinUITarget));
        }

        public void Spawn(Vector3 position)
        {
            Coin coin = _gameFactory.CreateCoin(position);
            coin.Initialize(_coinUiTarget);
        }
    }
}