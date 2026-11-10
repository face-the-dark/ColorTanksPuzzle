using System;
using UnityEngine;

namespace _Game.Code.UI.Window
{
    public abstract class UserInterfaceWindow : MonoBehaviour
    {
        [SerializeField] private Canvas _windowCanvas;
        [SerializeField] private Canvas _rootCanvas;

        public event Action Opened;
        public event Action Closed;

        protected void InvokeOpen()
        {
            Opened?.Invoke();
        }

        protected void InvokeClose()
        {
            Closed?.Invoke();
        }

        public void EnableCanvases()
        {
            _rootCanvas.enabled = true;
            _windowCanvas.enabled = true;
        }

        public void DisableCanvases()
        {
            _rootCanvas.enabled = false;
            _windowCanvas.enabled = false;
        }
    }
}