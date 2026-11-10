using System;
using _Game.Code.Bonuses;
using _Game.Code.Configurations.Bonuses;
using _Game.Code.Infrastructure.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.Window
{
    public class BuyingWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _closeButton;
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
            _closeButton.onClick.AddListener(Close);
            
            InvokeOpen();
            
            _bonus = bonus;
            
            UpdateWindow();
            _buyButton.onClick.AddListener(Buy);
        }

        private void Close()
        {
            _closeButton.onClick.RemoveListener(Close);
            
            InvokeClose();
            
            _bonus = Bonus.None;
            _buyButton.onClick.RemoveListener(Buy);
        }

        private void UpdateWindow()
        {
            BonusConfiguration bonusConfiguration = _bonusesConfiguration.GetBonusConfiguration(_bonus);

            if (bonusConfiguration != null)
            {
                _bonusIcon.sprite = bonusConfiguration.Icon;
                _bonusCountText.text = $"x{bonusConfiguration.Count.ToString()}";
                _bonusNameText.text = bonusConfiguration.Name;
                _bonusPriceText.text = bonusConfiguration.Cost.ToString();
            }
        }

        private void Buy()
        {
            if (_bonusBuyer.TryBuy(_bonus)) 
                Close();
        }
    }
}