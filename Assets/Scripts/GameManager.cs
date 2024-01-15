using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver, levelCompleted;
    [SerializeField] uint chickenTarget = 2;
    public TextMeshProUGUI scoreText;

    private PlayerController playerController;
    [SerializeField] GameObject player;
    public GameData gameData;
    public SaveManager saveManager;

    [SerializeField] GameObject gameoverPanel;
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Game Manager es null");
            }
            return instance;
        }
    }

    /* 
     * private void Awake()
     {
         if (instance == null)
         {
             instance = this;
             DontDestroyOnLoad(gameObject);
         }
         else
         {
             Destroy(gameObject);
         }
     }
    */

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // gameData = saveManager.LoadGame();
        if (gameData == null)
        {
            gameData = new GameData();
        }
        else
        {
            foreach (int level in gameData.unlockedLevels)
            {
                UnlockLevel(level);
            }
        }

        UnlockLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        LevelCompleted();
        GameOver();
        Score();
    }

    public void Score()
    {
        if (playerController.chickenList.Count != 0)
        {
            scoreText.text = " " + playerController.chickenList.Count.ToString();
        }
    }

    public void LevelCompleted()
    {
        if (playerController.chickenList.Count == chickenTarget)
        {
            levelCompleted = true;
            if (playerController.hasHittedNextLevel)
            {
                AudioManager.instance.PlaySFX("LevelCompleted");
                int currentLevel = SceneManager.GetActiveScene().buildIndex;
                saveManager.SaveGame(currentLevel);
                // saveManager.SaveToCloud(currentLevel);
            }

        }
    }

    public void GameOver()
    {
        if (gameOver == true)
        {
            gameoverPanel.SetActive(true);
        }
    }

    public void UnlockLevel(int level)
    {
        if (!gameData.unlockedLevels.Contains(level))
        {
            gameData.unlockedLevels.Add(level);
        }
    }


}
