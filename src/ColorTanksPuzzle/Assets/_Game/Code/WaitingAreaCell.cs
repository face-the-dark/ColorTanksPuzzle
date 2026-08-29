using UnityEngine;

namespace _Game.Code
{
    public class WaitingAreaCell : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        
        public Vector3 PointPosition => _point.position;
    }
}