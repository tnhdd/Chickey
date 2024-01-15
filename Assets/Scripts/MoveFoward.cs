using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb != null)
        {
            Vector3 forwardForce = transform.forward * moveSpeed;
            rb.AddForce(forwardForce, ForceMode.Force);
        }
    }
}
