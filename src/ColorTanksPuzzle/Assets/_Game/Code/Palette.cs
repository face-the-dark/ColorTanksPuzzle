using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Palette")]
    public class Palette : ScriptableObject
    {
        [SerializeField] private List<Color> _colors;
        
        public List<Color> Colors => _colors;
    }
}