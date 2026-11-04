using UnityEngine;

namespace _Game.Code.Bullets
{
    public class BulletDyer : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        public void ApplyColor(Color color)
        {
            _renderer.material = new Material(_renderer.sharedMaterial)
            {
                color = color
            };
        }
    }
}