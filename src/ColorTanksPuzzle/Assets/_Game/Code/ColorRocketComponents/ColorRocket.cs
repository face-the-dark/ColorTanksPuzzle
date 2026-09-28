using System;
using UnityEngine;

namespace _Game.Code.ColorRocketComponents
{
    public class ColorRocket : MonoBehaviour
    {
        [SerializeField] private Transform[] _parts;
        
        private Color _color;
        private Animator _animator;

        public event Action<ColorRocket> RocketArrived;
        
        public Color Color => _color;

        public void Initialize(Color pixelColor)
        {
            _color = pixelColor;
            
            foreach (Transform part in _parts)
            {
                Renderer rendererComponent = part.GetComponent<Renderer>();

                if (rendererComponent is null)
                    throw new ArgumentNullException(nameof(rendererComponent));
                
                rendererComponent.material.color = pixelColor;
            }
        }

        public void OnRocketArrived()
        {
            RocketArrived?.Invoke(this);
        }
    }
}