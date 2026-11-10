using System;
using UnityEngine;

namespace _Game.Code.UI.Window
{
    public class WindowEvent : MonoBehaviour
    {
        public event Action CloseAnimationCompleted;

        public void OnCloseAnimationCompleted() => 
            CloseAnimationCompleted?.Invoke();
    }
}