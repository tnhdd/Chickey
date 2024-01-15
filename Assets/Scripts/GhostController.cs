using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] float speedForward = 2.0f;
    [SerializeField] float waitTime;
    float currentWaitTime;
    [SerializeField] float xValues, zValues;
    private Vector3 moveSpot;

    [SerializeField] GameObject chicken;
    private bool isWaiting, isDelayed;
    private ChickenManager chickenManager;
    
    private void Awake()
    {
        moveSpot = GetNewPosition();

    }
    private void Start()
    {
        GameObject chickenObject = GameObject.Find("Chicken");
       
        if (chickenObject != null)
        {
            chickenManager = GameObject.Find("Chicken").GetComponent<ChickenManager>();
        }

    }

    private void Update()
    {
        if (!isWaiting)
        {
            if (isDelayed)
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    isDelayed = false;
                }
            }
            else
            {
                Rotate();
                Patroling();
            }

        }
    }

    Vector3 GetNewPosition()
    {
        float randomX = Random.Range(-xValues, xValues);
        float randomZ = Random.Range(-zValues, zValues);

        Vector3 newPosition = new Vector3(randomX, 0f, randomZ);
        return newPosition;
    }

    // Patrolling around the map
    void Patroling()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speedForward * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveSpot) <= 0.2f)
        {
            if (currentWaitTime <= 0)
            {
                moveSpot = GetNewPosition();
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= waitTime;
            }
        }
    }

    // Rotate Ghost to facing the directing its moving
    void Rotate()
    {
        Vector3 targetDirection = moveSpot - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);

        Quaternion newRotaion = Quaternion.LookRotation(newDirection);
        newRotaion.eulerAngles = new Vector3(0f, newRotaion.eulerAngles.y, 0f);

        transform.rotation = newRotaion;
    }

    private IEnumerator StartWaiting(float delayTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(delayTime);
        isWaiting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chicken") && chicken.GetComponent<ChickenManager>().hasBeenAddedToList)
        {
            AudioManager.instance.PlaySFX("ChickenDeath");
            StartCoroutine(StartWaiting(3f)); // Wait for 3 seconds
            Vector3 spawnPosition = new Vector3(Random.Range(-xValues, xValues), 0f, Random.Range(-zValues, zValues));
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(chicken, spawnPosition, randomRotation);
           
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            //AudioManager.instance.PlaySFX("GameOver");
        }
    }
}
