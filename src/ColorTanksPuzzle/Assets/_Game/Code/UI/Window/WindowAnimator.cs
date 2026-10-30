using UnityEngine;

namespace _Game.Code.UI.Window
{
    public class WindowAnimator : MonoBehaviour
    {
        private static readonly int OpenKey = Animator.StringToHash("Open");
        private static readonly int CloseKey = Animator.StringToHash("Close");

        [SerializeField] private Animator _targetWindowAnimator;
        
        private UserInterfaceWindow _userInterfaceWindow;

        private void Awake()
        {
            _userInterfaceWindow = GetComponent<UserInterfaceWindow>();
        }

        private void OnEnable()
        {
            _userInterfaceWindow.Opened += OnOpened;
            _userInterfaceWindow.Closed += OnClosed;
        }

        private void OnDisable()
        {
            _userInterfaceWindow.Opened -= OnOpened;
            _userInterfaceWindow.Closed -= OnClosed;
        }

        public void OnCloseAnimationCompleted()
        {
            _userInterfaceWindow.DisableCanvas();
        }

        private void OnOpened() => 
            _targetWindowAnimator.SetTrigger(OpenKey);

        private void OnClosed() => 
            _targetWindowAnimator.SetTrigger(CloseKey);
    }
}