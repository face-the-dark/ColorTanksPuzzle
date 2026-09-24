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
            _playerInput.Player.FirstBonusAction.performed += OnFirstBonusActivated;
        }

        public event Action<Vector2> Clicked;
        public event Action FirstBonusUsed;

        public void Dispose()
        {
            _playerInput.Player.ClickAction.performed -= OnClicked;
            _playerInput.Player.FirstBonusAction.performed -= OnFirstBonusActivated;
            
            _playerInput.Disable();
        }

        private void OnClicked(InputAction.CallbackContext callbackContext)
        {
            Vector2 position = _playerInput.Player.PositionAction.ReadValue<Vector2>();
            
            Clicked?.Invoke(position);
        }

        private void OnFirstBonusActivated(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                FirstBonusUsed?.Invoke();
        }
    }
}