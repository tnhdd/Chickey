using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Color = UnityEngine.Color;

public class OrcController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public NavMeshAgent navMeshAgent;

    public Transform player;
    public Transform centerPoint;
    public GameObject chicken;
    [SerializeField] GameObject hitEffect;

    public LayerMask groudLayer, chickenLayer;

    private PlayerController playerController;

    //Patroling
    float timeToStay = 0;
    [SerializeField] float patrolDelay;
    bool isDelayed = false;

    //States`
    public float sightRange, attackRange;
    private bool chickenInSightRange;

    public float rangeToWalk;
    [SerializeField] float patrolingSpeed;
    [SerializeField] float runnningSpeed;

    [SerializeField] float kbForce;
    bool isAttacked = false;
    [SerializeField] float timeBetweenDie;

    Vector3 pushbackDirection = Vector3.forward;
    ShovelController shovelController;

    private GameManager gameManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        patrolingSpeed = navMeshAgent.speed;
        shovelController = GameObject.Find("Shovel").GetComponent<ShovelController>();
        
    }


    void Update()
    {
        if (!gameManager.gameOver && !gameManager.levelCompleted)
        {
            chickenInSightRange = Physics.CheckSphere(transform.position, sightRange, chickenLayer);

            if (!isAttacked) Patroling();
            if (chickenInSightRange) ChaseChicken();
            HittedByShovel();
        }
    }

    private void Patroling()
    {
        // set position in the y axis always to 0
        Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = newPosition;

        if (isDelayed)
        {
            patrolDelay -= Time.deltaTime;
            if (patrolDelay <= 0)
            {
                isDelayed = false;
                navMeshAgent.isStopped = false;
            }
        }
        else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) //
        {
            if (timeToStay <= 0)
            {
                Vector3 point;
                if (RandomPoint(centerPoint.position, rangeToWalk, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    navMeshAgent.SetDestination(point);
                    timeToStay = Random.Range(3f, 5f);
                }
            }
            else
            {
                timeToStay -= Time.deltaTime;
            }
        }

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }

    }

    private void ChaseChicken()
    {
        if (playerController.hasChickenInList == true) // Check if there is any chicken in the list yet
        {
            float distanceToFollowBehind = 0.5f;
            navMeshAgent.speed = runnningSpeed;

            if (playerController.chickenList != null && playerController.chickenList.Count >= 1)
            {
                GameObject chickenToChase = playerController.chickenList[playerController.chickenList.Count - 1];
                Vector3 newPosition = chickenToChase.transform.position - (transform.forward * distanceToFollowBehind);
                navMeshAgent.SetDestination(newPosition);
            }
        }
        else
        {
            navMeshAgent.speed = patrolingSpeed;
        }

    }

    private void HittedByShovel()
    {
        if (shovelController.attackedOrc == true)
        {
            navMeshAgent.isStopped = true;
            animator.SetTrigger("die");
            Vector3 pushbackDirection = transform.position - player.transform.position;
            pushbackDirection.Normalize();
            AttackedByPlayer(pushbackDirection);
            shovelController.attackedOrc = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chicken"))
        {
            isDelayed = true;
            patrolDelay = 3f;
            navMeshAgent.isStopped = true;

            Vector3 spawnPosition = new Vector3(Random.Range(-rangeToWalk, rangeToWalk), 0f, Random.Range(-rangeToWalk, rangeToWalk));
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(chicken, spawnPosition, randomRotation);
        }
    }

    private void AttackedByPlayer(Vector3 pushbackDirection)
    {
        transform.LookAt(player); // Look at player after hitted
        rb.AddForce(pushbackDirection * kbForce, ForceMode.VelocityChange);
        animator.SetBool("walking", false);
        isAttacked = true;
        Invoke(nameof(Revive), timeBetweenDie);
        Instantiate(hitEffect, transform.position, Quaternion.identity); // spawn hit effect
    }


    private void Revive() // Resurrection 
    {
        isAttacked = false;
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = false;
        StartCoroutine(DelayToIdleState());
    }

    IEnumerator DelayToIdleState() // Delay to play idle stage after death
    {
        yield return new WaitForSeconds(3);

        animator.SetTrigger("idleT");
    }

    // Get random point on the map to move
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
