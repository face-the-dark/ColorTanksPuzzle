using _Game.Code.PersistenceProgress;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinsView : MonoBehaviour
    {
        private PlayerDataService _playerDataService;
        
        private TextMeshProUGUI _moneyText;

        [Inject]
        public void Construct(PlayerDataService coinsCounter) => 
            _playerDataService = coinsCounter;

        private void Awake() => 
            _moneyText = GetComponent<TextMeshProUGUI>();

        private void OnEnable() => 
            _playerDataService.MoneyChanged += UpdateText;

        private void OnDisable() => 
            _playerDataService.MoneyChanged -= UpdateText;

        private void UpdateText(int amount) => 
            _moneyText.text = amount.ToString();
    }
}