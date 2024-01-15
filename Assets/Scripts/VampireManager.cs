using UnityEngine;
using UnityEngine.AI;

public class VampireManager : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public NavMeshAgent navMeshAgent;

    public Transform player;
    public Transform centrePoint;

    public LayerMask groundLayer, playerLayer;

    float timeToStay = 0;
    [SerializeField] float walkRange;

    [SerializeField] float sightRange, attackRange;
    bool playerInSightRange, playerInAttackRange;
    bool attacking;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireBallSpeed;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!gameManager.gameOver && !gameManager.levelCompleted)
        {
            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }

    }

    void Patrolling()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) //done with path
        {
            if (timeToStay <= 0)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, walkRange, out point)) //pass in our centre point and radius of area
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

        if (navMeshAgent.velocity.magnitude > 0.05f)
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }

    void ChasePlayer()
    {
        float distanceToFollow = 2f;
        Vector3 newPosition = player.transform.position - (transform.forward * distanceToFollow);
        navMeshAgent.SetDestination(newPosition);
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("shoot");
        animator.SetBool("walking", false);
        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!attacking)
        {
            GameObject fireBallGameObject = Instantiate(fireBall, firePoint.transform.position, firePoint.rotation);
            Rigidbody fireRb = fireBallGameObject.GetComponent<Rigidbody>();
            fireRb.AddForce(transform.forward * fireBallSpeed, ForceMode.Impulse);
            attacking = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
    private void ResetAttack()
    {
        attacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shovel"))
        {
            Debug.Log("Hitted by shovel");
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitted by player");
        }
    }

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
