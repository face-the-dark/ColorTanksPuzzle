using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI.Window
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UserInterfaceWindow : MonoBehaviour
    {
        [SerializeField] protected Button CloseButton;
        
        [SerializeField] private Canvas _rootCanvas;
        
        private Canvas _canvas;

        public event Action Opened;
        public event Action Closed;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            if (CloseButton != null)
                CloseButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            if (CloseButton != null)
                CloseButton.onClick.RemoveListener(Close);
        }

        public void Open()
        {
            EnableCanvas();

            OnOpen();

            Opened?.Invoke();
        }

        public void Close()
        {
            OnClose();

            Closed?.Invoke();
        }

        public void EnableCanvas()
        {
            _rootCanvas.enabled = true;
            _canvas.enabled = true;
        }

        public void DisableCanvas()
        {
            _rootCanvas.enabled = false;
            _canvas.enabled = false;
        }

        protected abstract void OnOpen();
        protected abstract void OnClose();
    }
}