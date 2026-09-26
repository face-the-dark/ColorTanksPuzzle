using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Code.Players
{
    public class InputReader : IDisposable
    {
        private readonly PlayerInput _playerInput;

        public InputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            
            _playerInput.Enable();

            _playerInput.Player.ClickAction.performed += OnClicked;
            _playerInput.Player.ExpansionBonusAction.performed += OnExpansionBonusActivated;
            _playerInput.Player.FreezeSplineBonusAction.performed += OnFreezeSplineBonusActivated;
            _playerInput.Player.ScrificeBonusAction.performed += OnSacrificeBonusActivated;
        }

        public event Action<Vector2> Clicked;
        public event Action ExpansionBonusUsed;
        public event Action FreezeSplineBonusUsed;
        public event Action SacrificeBonusUsed;

        public void Dispose()
        {
            _playerInput.Player.ClickAction.performed -= OnClicked;
            _playerInput.Player.ExpansionBonusAction.performed -= OnExpansionBonusActivated;
            _playerInput.Player.FreezeSplineBonusAction.performed -= OnFreezeSplineBonusActivated;
            _playerInput.Player.ScrificeBonusAction.performed -= OnSacrificeBonusActivated;
            
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
                ExpansionBonusUsed?.Invoke();
        }
        
        private void OnFreezeSplineBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                FreezeSplineBonusUsed?.Invoke();
        }
        
        private void OnSacrificeBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                SacrificeBonusUsed?.Invoke();
        }
    }
}