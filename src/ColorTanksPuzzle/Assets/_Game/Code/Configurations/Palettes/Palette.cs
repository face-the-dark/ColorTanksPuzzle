using System.Collections.Generic;
using UnityEngine;

namespace _Game.Code.Configurations.Palettes
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Configurations/Palette")]
    public class Palette : ScriptableObject
    {
        [SerializeField] private List<Color> _colors;
        
        public List<Color> Colors => _colors;
    }
}