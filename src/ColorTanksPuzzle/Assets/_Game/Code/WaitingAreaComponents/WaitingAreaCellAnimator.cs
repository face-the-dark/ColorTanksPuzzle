using UnityEngine;

namespace _Game.Code.WaitingAreaComponents
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WaitingAreaCell))]
    public class WaitingAreaCellAnimator : MonoBehaviour
    {
        private static readonly int Spawn = Animator.StringToHash("Spawn");
        
        private Animator _animator;

        private WaitingAreaCell _waitingAreaCell;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _waitingAreaCell = GetComponent<WaitingAreaCell>();
        }

        private void OnEnable()
        {
            _waitingAreaCell.Spawned += OnSpawned;
        }

        private void OnDisable()
        {
            _waitingAreaCell.Spawned -= OnSpawned;
        }

        private void OnSpawned()
        {
            _animator.SetTrigger(Spawn);
        }
    }
}