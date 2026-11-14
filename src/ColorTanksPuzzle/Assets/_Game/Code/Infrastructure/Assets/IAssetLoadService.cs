using System.Collections.Generic;
using _Game.Code.Bullets;
using _Game.Code.Coins;
using _Game.Code.ColorRocketComponents;
using _Game.Code.Configurations;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Configurations.Palettes;
using _Game.Code.Pixels;
using _Game.Code.TankComponents;
using _Game.Code.WaitingAreaComponents;

namespace _Game.Code.Infrastructure.Assets
{
    public interface IAssetLoadService
    {
        List<DifficultyConfiguration> LoadDifficultyConfigurations();
        Palette LoadPalette();
        Pixel LoadPixelPrefab();
        Tank LoadTankPrefab();
        Bullet LoadBulletPrefab();
        WaitingAreaCell LoadWaitingAreaCellPrefab();
        BonusesConfiguration LoadBonusesConfiguration();
        TankConfiguration LoadTankConfiguration();
        ColorRocket LoadColorRocketPrefab();
        Coin LoadCoinPrefab();
    }
}