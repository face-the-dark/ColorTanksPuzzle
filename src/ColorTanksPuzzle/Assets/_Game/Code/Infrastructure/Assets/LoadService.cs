using System.Collections.Generic;
using System.Linq;
using _Game.Code.Bullets;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Configurations.Difficulty;
using _Game.Code.Configurations.Palettes;
using _Game.Code.Pixels;
using _Game.Code.Tanks;
using _Game.Code.WaitingAreaComponents;
using UnityEngine;

namespace _Game.Code.Infrastructure.Assets
{
    public class LoadService
    {
        public List<DifficultyConfiguration> LoadDifficultyConfigurations() =>
            Resources
                .LoadAll<DifficultyConfiguration>(AssetPath.DifficultyConfigurationPath)
                .ToList();

        public Palette LoadPalette() => 
            Resources.Load<Palette>(AssetPath.PalettePath);

        public Pixel LoadPixel() => 
            Resources.Load<Pixel>(AssetPath.PixelPrefabPath);

        public Tank LoadTank() => 
            Resources.Load<Tank>(AssetPath.TankPrefabPath);

        public Bullet LoadBullet() => 
            Resources.Load<Bullet>(AssetPath.BulletPrefabPath);

        public WaitingAreaCell LoadWaitingAreaCell() => 
            Resources.Load<WaitingAreaCell>(AssetPath.WaitingAreaCellPrefabPath);

        public BonusesConfiguration LoadBonusesConfiguration() => 
            Resources.Load<BonusesConfiguration>(AssetPath.BonusesConfigurationPath);
    }
}