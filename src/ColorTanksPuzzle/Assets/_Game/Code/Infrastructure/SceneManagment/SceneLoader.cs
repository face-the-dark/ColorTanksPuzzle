using UnityEngine.SceneManagement;

namespace _Game.Code.Infrastructure.SceneManagment
{
    public class SceneLoader
    {
        private const string GameSceneName = "Game";
        private const string MenuSceneName = "Menu";

        public void LoadGameScene() => 
            SceneManager.LoadScene(GameSceneName);

        public void LoadMenuScene() => 
            SceneManager.LoadScene(MenuSceneName);
    }
}