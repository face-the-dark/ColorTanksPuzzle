using _Game.Code.Bullets;
using _Game.Code.Coins;
using _Game.Code.ColorRocketComponents;
using _Game.Code.TankComponents;
using UnityEngine;

namespace _Game.Code.Infrastructure
{
    public interface IGameFactory
    {
        Bullet CreateBullet(Vector3 position);
        Tank CreateTank(Transform spawningLane);
        ColorRocket CreateColorRocket();
        Coin CreateCoin(Vector3 position);
    }
}