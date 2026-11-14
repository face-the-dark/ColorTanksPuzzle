using _Game.Code.TankComponents;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI.View
{
    public class TankHealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Tank _tank;

        private void OnEnable() => 
            _tank.HealthValueChanged += OnHpChanged;

        private void OnDisable() => 
            _tank.HealthValueChanged -= OnHpChanged;

        private void OnHpChanged(int value) => 
            _text.text = value.ToString();
    }
}