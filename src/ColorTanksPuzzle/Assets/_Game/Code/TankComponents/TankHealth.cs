using System;
using UnityEngine;

namespace _Game.Code.TankComponents
{
    public class TankHealth : MonoBehaviour
    {
        private int _value;
        
        public int Value => _value;

        public event Action<int> ValueChanged;
        public event Action ValueEnded;
        
        public void Initialize(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            _value = value;
            
            ValueChanged?.Invoke(_value);
        }

        public void Decrease()
        {
            if (_value <= 0)
                throw new InvalidOperationException(nameof(_value));
            
            _value--;
            
            ValueChanged?.Invoke(_value);
            
            if (_value <= 0)
                ValueEnded?.Invoke();
        }
    }
}