using System;
using _Game.Code.Bonuses;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace _Game.Code.Players
{
    public class InputReader : IDisposable
    {
        private readonly PlayerInput _playerInput;

        [Inject]
        public InputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            
            _playerInput.Enable();

            _playerInput.Player.ClickAction.performed += OnClicked;
            _playerInput.Player.ExpansionBonusAction.performed += OnExpansionBonusActivated;
            _playerInput.Player.FreezeSplineBonusAction.performed += OnFreezeSplineBonusActivated;
            _playerInput.Player.ScrificeBonusAction.performed += OnSacrificeBonusActivated;
            _playerInput.Player.ColorRocketBonusAction.performed += OnColorRocketBonusActivated;
        }

        public event Action<Vector2> Clicked;
        public event Action<Bonus> ExpansionBonusUsed;
        public event Action<Bonus> SwitchTanksBonusUsed;
        public event Action<Bonus> SacrificeBonusUsed;
        public event Action<Bonus> ColorRocketBonusUsed;

        public void Dispose()
        {
            _playerInput.Player.ClickAction.performed -= OnClicked;
            _playerInput.Player.ExpansionBonusAction.performed -= OnExpansionBonusActivated;
            _playerInput.Player.FreezeSplineBonusAction.performed -= OnFreezeSplineBonusActivated;
            _playerInput.Player.ScrificeBonusAction.performed -= OnSacrificeBonusActivated;
            _playerInput.Player.ColorRocketBonusAction.performed -= OnColorRocketBonusActivated;
            
            _playerInput.Disable();
        }
        
        private void OnClicked(InputAction.CallbackContext callbackContext)
        {
            Vector2 position = _playerInput.Player.PositionAction.ReadValue<Vector2>();
            
            Clicked?.Invoke(position);
        }

        private void OnExpansionBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                ExpansionBonusUsed?.Invoke(Bonus.Expansion);
        }
        
        private void OnFreezeSplineBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                SwitchTanksBonusUsed?.Invoke(Bonus.SwitchTanks);
        }
        
        private void OnSacrificeBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                SacrificeBonusUsed?.Invoke(Bonus.Sacrifice);
        }
        
        private void OnColorRocketBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                ColorRocketBonusUsed?.Invoke(Bonus.ColorRocket);
        }
    }
}