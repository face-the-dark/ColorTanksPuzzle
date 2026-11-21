using System;
using System.Collections.Generic;

namespace _Game.Code.Spawners.Lines
{
    public interface ITankGroupMoveAnimator
    {
        void MoveAll(IReadOnlyList<MoveStep> steps, float duration, Action onComplete);
        void Kill();
    }
}