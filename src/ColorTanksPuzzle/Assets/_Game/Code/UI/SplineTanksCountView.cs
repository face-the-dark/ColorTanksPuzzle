using System;
using _Game.Code.Players;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SplineTanksCountView : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        
        private TankDispatcher _tankDispatcher;

        [Inject]
        public void Construct(TankDispatcher tankDispatcher) => 
            _tankDispatcher = tankDispatcher ?? throw new ArgumentNullException(nameof(tankDispatcher));

        private void Awake() => 
            _text =  GetComponentInChildren<TextMeshProUGUI>();

        private void OnEnable() => 
            _tankDispatcher.TanksCountChanged += OnTanksCountChanged;

        private void OnDisable() => 
            _tankDispatcher.TanksCountChanged -= OnTanksCountChanged;

        private void OnTanksCountChanged(string text) => 
            _text.text = text;
    }
}