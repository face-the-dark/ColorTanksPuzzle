using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UserInterfaceWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            Close();
        }

        private void OnEnable() =>
            _closeButton.onClick.AddListener(Close);

        private void OnDisable() =>
            _closeButton.onClick.RemoveListener(Close);

        protected void Open()
        {
            _canvas.enabled = true;
            
            OnOpen();
        }

        protected void Close()
        {
            _canvas.enabled = false;
            
            OnClose();
        }

        protected abstract void OnClose();

        protected abstract void OnOpen();
    }
}