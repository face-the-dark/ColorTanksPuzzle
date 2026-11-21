using System;
using UnityEngine;

namespace _Game.Code.TankComponents
{
    public interface ITankJumpAnimator
    {
        void JumpTo(Transform target, Vector3 position, float power, float duration, Action onComplete = null);
        void Kill();
        ITankJumpAnimator Clone();
    }
}