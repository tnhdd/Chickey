using System.Collections.Generic;
using System.IO;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : Menu
{
    [SerializeField] private string fileName;
    public int levelUnlockedFromSaveSlot;
    [SerializeField] SaveManager saveManager;
    [SerializeField] Button[] levelButton;
    private int levelUnlocked;
    
    
    [System.Serializable]
    private class SaveData
    {
        public int lastSavedLevel;
    }

    private void Awake()
    {
        saveManager = GameObject.Find("Save Manager").GetComponent<SaveManager>();
        Debug.Log(saveManager.levelUnlocked);
    }
    void Start()
    {
        
        for (int i = 0; i < levelButton.Length; i++)
        {
            if (i < saveManager.levelUnlocked)
            {
                levelButton[i].interactable = true;
            }
            else
            {
                levelButton[i].interactable = false;
            }
        }
    }

  

    public int LoadLastSavedLevel()
    {
        string path = GetSavePath();

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonData);
            return loadedData.lastSavedLevel;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return 0; // Or any default level you want to set.
        }
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
