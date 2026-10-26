using _Game.Code.Generators.Data;
using UnityEngine;

namespace _Game.Code.Tanks
{
    public class TankDyer : MonoBehaviour
    {
        [SerializeField] private TankPartDyer[] _tankParts;
        
        public void Initialize(TankData tankData)
        {
            foreach (TankPartDyer tankPartDyer in _tankParts) 
                tankPartDyer.ApplyColor(tankData.Color);
        }
    }
}