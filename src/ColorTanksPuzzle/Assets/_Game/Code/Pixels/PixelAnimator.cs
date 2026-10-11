using UnityEngine;

namespace _Game.Code.Pixels
{
    [RequireComponent(typeof(Animator))]
    public class PixelAnimator : MonoBehaviour
    {
        private static readonly int DieKey = Animator.StringToHash("Die");
        
        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void Die() => 
            _animator.SetTrigger(DieKey);
    }
}