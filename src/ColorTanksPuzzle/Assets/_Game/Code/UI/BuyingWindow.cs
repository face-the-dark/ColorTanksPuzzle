using System;
using _Game.Code.Bonuses;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Infrastructure.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    public class BuyingWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Image _bonusIcon;
        [SerializeField] private TextMeshProUGUI _bonusCountText;
        [SerializeField] private TextMeshProUGUI _bonusNameText;
        [SerializeField] private TextMeshProUGUI _bonusPriceText;

        private BonusBuyer _bonusBuyer;

        private BonusesConfiguration _bonusesConfiguration;

        private Bonus _bonus;

        [Inject]
        public void Construct(BonusBuyer bonusBuyer, AssetLoadService assetLoadService)
        {
            _bonusBuyer = bonusBuyer ?? throw new ArgumentNullException(nameof(bonusBuyer));
            
            if (assetLoadService == null)
                throw new ArgumentNullException(nameof(assetLoadService));
            
            _bonusesConfiguration = assetLoadService.LoadBonusesConfiguration();
        }

        public void Open(Bonus bonus)
        {
            Open();

            BonusConfiguration bonusConfiguration = _bonusesConfiguration.GetBonusConfiguration(bonus);

            _bonusIcon.sprite = bonusConfiguration.Icon;
            _bonusCountText.text = bonusConfiguration.Count.ToString();
            _bonusNameText.text = bonusConfiguration.Name;
            _bonusPriceText.text = bonusConfiguration.Cost.ToString();

            _bonus = bonus;
        }

        protected override void OnOpen()
        {
            _buyButton.onClick.AddListener(Buy);
        }

        protected override void OnClose()
        {
            _buyButton.onClick.RemoveListener(Buy);
        }

        private void Buy()
        {
            _bonusBuyer.Buy(_bonus);

            Close();
        }
    }
}