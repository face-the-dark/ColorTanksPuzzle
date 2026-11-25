using UnityEngine;

namespace _Game.Code.Configurations
{
    public class WaitingAreaCellsSpawnBoundaries : MonoBehaviour
    {
        [SerializeField] private Transform _leftUpBoundary;
        [SerializeField] private Transform _rightUpBoundary;
        
        public Transform LeftUpBoundary => _leftUpBoundary;
        public Transform RightUpBoundary => _rightUpBoundary;
    }
}