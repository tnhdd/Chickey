using DialogueEditor;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    Animator animator;

    GameManager gameManager;
    [SerializeField] NPCConversation nPCConversation;
    [SerializeField] GameObject shovel;
    bool hasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponentInParent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && hasTriggered == false)
        {
            hasTriggered = true;
            ConversationManager.Instance.StartConversation(nPCConversation);
            gameManager.LevelCompleted();
            AudioManager.instance.PlaySFX("");
            if (shovel != null)
            {
                shovel.SetActive(true);
            }

        }
    }
}
