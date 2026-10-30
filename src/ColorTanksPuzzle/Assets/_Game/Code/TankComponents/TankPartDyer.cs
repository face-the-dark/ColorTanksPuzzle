using UnityEngine;

namespace _Game.Code.TankComponents
{
    [RequireComponent(typeof(Renderer))]
    public class TankPartDyer : MonoBehaviour
    {
        [SerializeField] private int _materialIndex;
        
        private Renderer _rendererComponent;

        private void Awake() => 
            _rendererComponent = GetComponent<Renderer>();

        public void ApplyColor(Color color) => 
            _rendererComponent.materials[_materialIndex].color = color;
    }
}