using UnityEngine;

public class CryptDoorController : MonoBehaviour
{
    Animator animator;
    public float rotationSpeed = 45.0f;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.levelCompleted == true)
        {
            animator.SetBool("open", true);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -90, 0), rotationSpeed * Time.deltaTime);
        }
    }
}
