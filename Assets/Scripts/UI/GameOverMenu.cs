using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void ReloadButton()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Debug.Log("Restarting game...");
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quiting game...");
    }
}
