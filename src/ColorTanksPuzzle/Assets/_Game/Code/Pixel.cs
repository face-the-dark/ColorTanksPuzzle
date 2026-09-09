using UnityEngine;

namespace _Game.Code
{
    public class Pixel : MonoBehaviour
    {
        private Color _color;

        public Color Color => _color;

        private bool _isWillBeDestroyed = false;
        
        public bool IsWillBeDestroyed => _isWillBeDestroyed;

        private void Start()
        {
            _color = GetComponent<Renderer>().material.color;
        }

        public void MarkForDestruction()
        {
            _isWillBeDestroyed = true;
        }
    }
}