using _Game.Code.Bonuses;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    public class BuyingWindow : UserInterfaceWindow
    {
        [SerializeField] private Button _buyButton;

        private BonusBuyer _bonusBuyer;
        
        private Bonus _bonus;
        
        [Inject]
        public void Construct(BonusBuyer bonusBuyer) => 
            _bonusBuyer = bonusBuyer;

        public void Open(Bonus bonus)
        {
            Open();
            
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