using _Game.Code.Bonuses;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Canvas))]
    public class BuyingWindow : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _closeButton;
        
        private Canvas _canvas;

        private BonusBuyer _bonusBuyer;
        
        private Bonus _bonus;
        
        [Inject]
        public void Construct(BonusBuyer bonusBuyer) => 
            _bonusBuyer = bonusBuyer;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            Close();
        }

        private void OnEnable() => 
            _closeButton.onClick.AddListener(Close);

        private void OnDisable() => 
            _closeButton.onClick.RemoveListener(Close);

        public void Open(Bonus bonus)
        {
            _bonus = bonus;
            
            _canvas.enabled = true;

            _buyButton.onClick.AddListener(Buy);
        }

        private void Close()
        {
            _canvas.enabled = false;
            
            _buyButton.onClick.RemoveListener(Buy);
        }

        private void Buy()
        {
            _bonusBuyer.Buy(_bonus);
            
            Close();
        }
    }
}