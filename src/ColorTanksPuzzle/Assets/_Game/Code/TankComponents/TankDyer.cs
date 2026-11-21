using UnityEngine;

namespace _Game.Code.TankComponents
{
    public class TankDyer : MonoBehaviour
    {
        [SerializeField] private TankPartDyer[] _tankParts;
        
        public void Initialize(Color color)
        {
            foreach (TankPartDyer tankPartDyer in _tankParts) 
                tankPartDyer.ApplyColor(color);
        }
    }
}