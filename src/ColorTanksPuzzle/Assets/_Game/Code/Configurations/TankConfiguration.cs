using UnityEngine;

namespace _Game.Code.Configurations
{
    [CreateAssetMenu(fileName = "TankConfiguration", menuName = "Configurations/Tank Configuration")]
    public class TankConfiguration : ScriptableObject
    {
        [SerializeField] private LayerMask _tankLayer;
        [SerializeField] private float _tankSpeed = 5f;
        [SerializeField] float _speedModifier = 2f;
        
        public LayerMask TankLayer => _tankLayer;
        public float TankSpeed => _tankSpeed;
        public float SpeedModifier => _speedModifier;
    }
}