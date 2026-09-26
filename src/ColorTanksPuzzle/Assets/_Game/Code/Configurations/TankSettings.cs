using UnityEngine;

namespace _Game.Code.Configurations
{
    [CreateAssetMenu(fileName = "TankSettings", menuName = "Configurations/Tank Settings")]
    public class TankSettings : ScriptableObject
    {
        [SerializeField] private LayerMask _tankLayer;
        
        public LayerMask TankLayer => _tankLayer;
    }
}