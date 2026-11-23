using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int menuScene;
    public int gameScene;

    public void ToMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void ToGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
