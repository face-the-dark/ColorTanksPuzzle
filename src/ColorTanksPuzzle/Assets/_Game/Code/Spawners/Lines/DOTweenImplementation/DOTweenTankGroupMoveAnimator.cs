using System;
using System.Collections.Generic;
using DG.Tweening;

namespace _Game.Code.Spawners.Lines.DOTweenImplementation
{
    public class DOTweenTankGroupMoveAnimator : ITankGroupMoveAnimator
    {
        private Sequence _sequence;

        public void MoveAll(IReadOnlyList<MoveStep> steps, float duration, Action onComplete)
        {
            Kill();

            _sequence = DOTween.Sequence();

            foreach (MoveStep step in steps)
            {
                _sequence.Join(step.Target
                        .DOMove(step.Destination, duration)
                        .SetEase(Ease.Linear));
            }

            _sequence.OnComplete(() => onComplete?.Invoke());
        }

        public void Kill()
        {
            _sequence?.Kill();
            _sequence = null;
        }
    }
}