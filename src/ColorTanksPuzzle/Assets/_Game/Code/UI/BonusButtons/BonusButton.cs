using _Game.Code.Bonuses;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game.Code.UI.BonusButtons
{
    [RequireComponent(typeof(Button))]
    public abstract class BonusButton : MonoBehaviour
    {
        private Button _button;

        private BonusActivator _bonusActivator;

        protected abstract Bonus Bonus { get; }

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
            _bonusActivator.ActivateBonus(Bonus);
    }
}