using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LoadGameMenu");
    }

    public void OptionMenu()
    {
        SceneManager.LoadScene("OptionMenuScene");
    }

    public void ExitGame()
    {
        Debug.Log("closing game");
        Application.Quit();
    }
}
