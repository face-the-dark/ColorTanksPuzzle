using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Code.Players
{
    public class InputReader : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public event Action<Vector2> Clicked;
        
        private void Awake() => 
            _playerInput = new PlayerInput();

        private void OnEnable()
        {
            _playerInput.Enable();

            _playerInput.Player.ClickAction.performed += OnClicked;
        }

        private void OnDisable()
        {
            _playerInput.Player.ClickAction.performed -= OnClicked;
            
            _playerInput.Disable();
        }

        private void OnClicked(InputAction.CallbackContext callbackContext)
        {
            Vector2 position = _playerInput.Player.PositionAction.ReadValue<Vector2>();
            
            Clicked?.Invoke(position);
        }
    }
}