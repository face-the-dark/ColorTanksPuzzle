using UnityEngine;

namespace _Game.Code.Bullets
{
    [RequireComponent(typeof(Renderer))]
    public class BulletDyer : MonoBehaviour
    {
        private Renderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void ApplyColor(Color color)
        {
            _renderer.material = new Material(_renderer.sharedMaterial)
            {
                color = color
            };
        }
    }
}