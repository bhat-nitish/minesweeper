using UnityEngine.SceneManagement;

namespace Base
{
    public enum GameScenes
    {
        None = 0,
        Menu = 1,
        Game = 2,
    }

    public static class GameNavigator
    {
        public static void NavigateToScene(GameScenes scene)
        {
            switch (scene)
            {
                case GameScenes.Menu:
                    SceneManager.LoadScene("Menu");
                    break;

                case GameScenes.Game:
                    SceneManager.LoadScene("Game");
                    break;
            }
        }
    }
}