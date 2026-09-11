using System;
using _Game.Code.Data;
using UnityEngine;

namespace _Game.Code.Tanks
{
    public class TankDyer : MonoBehaviour
    {
        [SerializeField] private Transform[] _tankParts;
        
        public void Initialize(TankData tankData)
        {
            foreach (Transform tankComponent in _tankParts)
            {
                Renderer rendererComponent = tankComponent.GetComponent<Renderer>();

                if (rendererComponent is null)
                    throw new ArgumentNullException(nameof(rendererComponent));
                
                rendererComponent.material.color = tankData.Color;
            }
        }
    }
}