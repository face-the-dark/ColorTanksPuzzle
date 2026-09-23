using _Game.Code.Configurations.Difficulty;
using UnityEngine;

namespace _Game.Code.Configurations.Levels
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Configurations/Level Settings")]
    public class LevelConfiguration : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Texture2D _picture;
        [SerializeField] private DifficultyMode _difficultyMode;
        [SerializeField] private int _seed;
        
        public int Id => _id;
        public Texture2D Picture => _picture;
        public DifficultyMode DifficultyMode => _difficultyMode;
        public int Seed => _seed;
    }
}