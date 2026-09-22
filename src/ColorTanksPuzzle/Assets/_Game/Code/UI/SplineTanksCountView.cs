using _Game.Code.Players;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    public class SplineTanksCountView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TankDispatcher _tankDispatcher;

        private void OnEnable() => 
            _tankDispatcher.TanksCountChanged += OnTanksCountChanged;

        private void OnDisable() => 
            _tankDispatcher.TanksCountChanged -= OnTanksCountChanged;

        private void OnTanksCountChanged(string text) => 
            _text.text = text;
    }
}