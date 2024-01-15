using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] KeyCode pauseKeyCode;
    bool isPaused = false;
    [SerializeField] GameObject howtoplayPanel;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(pauseKeyCode))
        {
            if (howtoplayPanel.activeInHierarchy == true)
            {
                howtoplayPanel.SetActive(false);
            }

            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("Menu Scene");
        Debug.Log("Back To Main Menu");
        Time.timeScale = 1f;
    }
}
