using System;
using _Game.Code.Bonuses;
using _Game.Code.PersistenceProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.BonusButtons
{
    [RequireComponent(typeof(Button))]
    public abstract class BonusButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bonusCountText;

        private Button _button;

        private BonusActivator _bonusActivator;
        private PlayerDataService _playerDataService;

        protected abstract Bonus Bonus { get; }

        [Inject]
        public void Construct(BonusActivator bonusActivator, PlayerDataService playerDataService)
        {
            _bonusActivator = bonusActivator ?? throw new ArgumentNullException(nameof(bonusActivator));
            _playerDataService = playerDataService 
                                            ?? throw new ArgumentNullException(nameof(playerDataService));
        }

        private void Awake() =>
            _button = GetComponent<Button>();

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            _playerDataService.BonusCountChanged += UpdateBonusCountText;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _playerDataService.BonusCountChanged -= UpdateBonusCountText;
        }

        private void OnButtonClick() => 
            _bonusActivator.ActivateBonus(Bonus);

        private void UpdateBonusCountText(Bonus bonus, int count)
        {
            if (Bonus == bonus)
                _bonusCountText.text = count.ToString();
        }
    }
}