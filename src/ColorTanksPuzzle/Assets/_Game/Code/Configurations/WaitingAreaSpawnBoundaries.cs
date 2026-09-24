using UnityEngine;

namespace _Game.Code.Configurations
{
    public class WaitingAreaSpawnBoundaries : MonoBehaviour
    {
        [SerializeField] private Transform _leftUpBoundary;
        [SerializeField] private Transform _rightUpBoundary;
        [SerializeField] private Transform _leftDownBoundary;
        [SerializeField] private Transform _rightDownBoundary;
        
        public Transform LeftUpBoundary => _leftUpBoundary;
        public Transform RightUpBoundary => _rightUpBoundary;
        public Transform LeftDownBoundary => _leftDownBoundary;
        public Transform RightDownBoundary => _rightDownBoundary;
    }
}