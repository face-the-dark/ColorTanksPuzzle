using _Game.Code.TankComponents;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI.View
{
    public class TankHealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Tank _tank;

        private void Start() => 
            OnHealthValueChanged(_tank.HealthValue);

        private void OnEnable() => 
            _tank.HealthValueChanged += OnHealthValueChanged;

        private void OnDisable() => 
            _tank.HealthValueChanged -= OnHealthValueChanged;

        private void OnHealthValueChanged(int value) => 
            _text.text = value.ToString();
    }
}