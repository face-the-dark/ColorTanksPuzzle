using UnityEngine.SceneManagement;

namespace _Game.Code.Infrastructure.SceneManagment
{
    public class SceneLoader
    {
        private const string GameSceneName = "Game";

        public void LoadGameScene() => 
            SceneManager.LoadScene(GameSceneName);
    }
}