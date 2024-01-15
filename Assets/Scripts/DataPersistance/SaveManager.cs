using System.Collections.Generic;
using System.IO;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private string filePath;
    [SerializeField] private string fileName;
    [HideInInspector] public int levelUnlocked;
    [HideInInspector] public bool isEmpty;
    [SerializeField] Button[] levelButton;
    [SerializeField] GameObject SaveSlot;
    [SerializeField] GameObject MainMenu;
    [System.Serializable]
    private class SaveData
    {
        public int unlockedLevel;
    }

    async void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        await UnityServices.InitializeAsync();
        
    }
    

    // Save game locally
    public void SaveGame(int level)
    {
        SaveData data = new SaveData();
        data.unlockedLevel = level;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    // Save to Cloud
    public async void SaveToCloud(int level)
    {
        var data = new Dictionary<string, object> { { "unlockedLevel", level } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async void LoadFromCloud()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "unlockedLevel" });

        if (playerData.TryGetValue("unlockedLevel", out var firstKey))
        {
            int levelUnlockedContinue = int.Parse(firstKey.Value.GetAs<string>());
            Debug.Log(levelUnlockedContinue.ToString() + " from Save manager");
            SceneManager.LoadScene(levelUnlockedContinue + 1);
        }
    }

   public async void OnLoadGameclicked()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "unlockedLevel" });

        if (playerData.TryGetValue("unlockedLevel", out var firstKey))
        {
            levelUnlocked = int.Parse(firstKey.Value.GetAs<string>());
            // SceneManager.LoadScene("Level Scene");
            Debug.Log(levelUnlocked.ToString() + " from Save manager");
            SaveSlot.SetActive(true);
            MainMenu.SetActive(false);

            for (int i = 0; i < levelButton.Length; i++)
            {
                if (i < levelUnlocked)
                {
                    levelButton[i].interactable = true;
                }
                else
                {
                    levelButton[i].interactable = false;
                }
            }
        }
    }

    public async void DeleteFromCloud()
    {
        await CloudSaveService.Instance.Data.Player.DeleteAsync("unlockedLevel");
    }

    public int LoadLastSavedLevel()
    {
        /*  string path = GetSavePath();

          if (File.Exists(path))
          {
              string jsonData = File.ReadAllText(path);
              SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonData);
              return loadedData.unlockedLevel;
          }
          else
          {
              Debug.LogWarning("Save file not found.");
              return 0; // Or any default level you want to set.
          }*/

        return levelUnlocked;
    }

    public void ResetSavedLevel(int level)
    {
        SaveData data = new SaveData();
        data.unlockedLevel = 1;
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
