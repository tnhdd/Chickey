using DialogueEditor;
using UnityEngine;
public class DiggerController : MonoBehaviour
{
    Animator animator;
    PlayerController playerController;
    GameManager gameManager;
    [SerializeField] GameObject levelCompletedText;
    [SerializeField] NPCConversation nPCConversation;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameManager.levelCompleted == true)
        {
            ConversationManager.Instance.StartConversation(nPCConversation);
        }
    }
}
