using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadButton;
    public int levelUnlocked;
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] SaveManager saveManager;


    private void Awake()
    {
        // checkIfHasSavedData();
        // saveManager.LoadFromCloud();
    }

    /*   private void checkIfHasSavedData()
       {
           string path = GetSavePath();
           if (File.Exists(path))
           {
               continueButton.interactable = true;
               loadButton.interactable = true;
           }
           else
           {
               continueButton.interactable = false;
               loadButton.interactable = false;
           }

       }*/
    public void OnNewGameClicked()
    {
        SceneManager.LoadSceneAsync("Level1");
    }
    public void OnLoadGameclicked()
    {
        SceneManager.LoadScene("Level Scene");
    }
    /*
        public void OnContinueGameClicked()
        {
            Debug.Log(saveManager.levelUnlocked);
            int lastSavedLevel = saveManager.LoadLastSavedLevel() + 1;
            SceneManager.LoadScene(lastSavedLevel);
        }*/

    /*private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }*/

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
