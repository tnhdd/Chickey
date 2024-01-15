using UnityEngine;

public class FireBallManager : MonoBehaviour
{
    [SerializeField] GameObject fireExplosion1;
    float boundaries = 14f;
    public bool hittedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // playerController = GetComponent<PlayerController>();   
        //playerAnimator = playerController.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > boundaries || transform.position.z > boundaries
            || transform.position.x < -boundaries || transform.position.z < -boundaries)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Shield"))
        {
            GameObject explosionEffect = Instantiate(fireExplosion1, transform.position, transform.rotation);
            //playerAnimator.SetTrigger("die");
            Destroy(gameObject, 1.5f);
            // Destroy the instantiated explosionObject after 1.2 seconds
            Destroy(explosionEffect, 1.5f);
        }

    }
}
