using _Game.Code.Bonuses;
using _Game.Code.Players;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class BonusButton : MonoBehaviour
    {
        [SerializeField] private Bonus _bonus;
        
        private Button _button;
        
        private BonusActivator _bonusActivator;

        [Inject]
        public void Construct(BonusActivator bonusActivator) => 
            _bonusActivator = bonusActivator;

        private void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnButtonClick);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnButtonClick);

        private void OnButtonClick() => 
            _bonusActivator.ActivateBonus(_bonus);
    }
}