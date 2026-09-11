using UnityEngine;

namespace _Game.Code.Pixels
{
    [RequireComponent(typeof(Renderer))]
    public class Pixel : MonoBehaviour
    {
        private Renderer _renderer;
        private bool _isWillBeDestroyed = false;

        public Color Color => _renderer.material.color;
        public bool IsWillBeDestroyed => _isWillBeDestroyed;

        private void Awake() => 
            _renderer = GetComponent<Renderer>();

        public void MarkForDestruction() => 
            _isWillBeDestroyed = true;
    }
}