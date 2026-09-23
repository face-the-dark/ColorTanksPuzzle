using _Game.Code.Tanks;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    public class TankHealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TankShooter _tankShooter;

        private void OnEnable() => 
            _tankShooter.HpChanged += OnHpChanged;

        private void OnDisable() => 
            _tankShooter.HpChanged -= OnHpChanged;

        private void OnHpChanged(int hp) => 
            _text.text = hp.ToString();
    }
}