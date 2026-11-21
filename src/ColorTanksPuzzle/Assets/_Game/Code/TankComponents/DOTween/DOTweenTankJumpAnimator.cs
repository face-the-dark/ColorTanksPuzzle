using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Code.TankComponents.DOTween
{
    public class DOTweenTankJumpAnimator : ITankJumpAnimator
    {
        private Tween _tween;

        public void JumpTo(Transform target, Vector3 position, float power, float duration, Action onComplete = null)
        {
            Kill();

            _tween = target
                .DOJump(position, power, 1, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void Kill()
        {
            _tween?.Kill();
            _tween = null;
        }

        public ITankJumpAnimator Clone()
        {
            return new DOTweenTankJumpAnimator();
        }
    }
}