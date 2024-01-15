using UnityEngine;
using UnityEngine.AI;

public class ChickenManager : MonoBehaviour
{
    Transform player;
    Animator animator;


    [SerializeField] NavMeshAgent navMeshAgent;

    private PlayerController playerController;
    [HideInInspector] public bool hasBeenAddedToList = false;
    bool shouldFollow;
    public int chickenOrder = 0;
    [SerializeField] float rotationSpeed;
    [SerializeField] float distanceToFollowBehind;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerController.chickenList.Count; i++)
        {
            if (gameObject == playerController.chickenList[i].gameObject)
            {
                chickenOrder = i;
            }
        }
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Set the Y component to 0f
        currentPosition.y = 0.05f;

        // Update the position of the GameObject
        transform.position = currentPosition;

    }

    private void FixedUpdate()
    {
        if (shouldFollow) FollowTarget();


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasBeenAddedToList)
        {
            playerController.chickenList.Add(gameObject);
            AudioManager.instance.PlaySFX("Score");
            hasBeenAddedToList = true;
            shouldFollow = true;
            
        }

        if (!gameManager.levelCompleted && playerController.hasChickenInList == true)
        {
            if (collision.gameObject.CompareTag("Orc") || collision.gameObject.CompareTag("Ghost"))
            {
                GameObject lastChiken = playerController.chickenList[playerController.chickenList.Count - 1];
                playerController.chickenList.RemoveAt(playerController.chickenList.Count - 1); // Remove last chiecken on the list
                Destroy(lastChiken); // Remove last chicken on the list
            }
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }


    void FollowTarget()
    {
        // Make the first chiken follow player
        if (chickenOrder == 0) // if there is no chicken yet in the list
        {
            Vector3 newPosition = player.transform.position - (transform.forward * distanceToFollowBehind);
            //newPosition.y = 0.05f;
            navMeshAgent.SetDestination(newPosition);
        }
        else // if the list already have chiken, make new one follows the last one
        {
            var chicken = playerController.chickenList[chickenOrder - 1];
            Vector3 newPosition = chicken.transform.position - (transform.forward * distanceToFollowBehind);
            //newPosition.y = 0.05f;
            navMeshAgent.SetDestination(newPosition);
        }

        // Play animation
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }
}


