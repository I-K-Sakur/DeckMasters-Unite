using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GameMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void SettingsMenu()
    {
        Debug.Log("Setting Working");
        Debug.Log("Setting Working");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the Game");
        Application.Quit();
    }
}
