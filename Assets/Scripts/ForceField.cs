using UnityEngine;

public class ForceField : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 30.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //RandomRotate();
    }

    private void RandomRotate()
    {
        // Generate random rotation angles for X, Y, and Z axes
        float randomX = Random.Range(0, 360);
        float randomY = Random.Range(0, 360);


        // Create a rotation using the random angles
        Quaternion randomRotation = Quaternion.Euler(randomX, randomY, 90);

        // Apply the rotation to the GameObject
        transform.rotation = Quaternion.RotateTowards(transform.rotation, randomRotation, rotationSpeed * Time.deltaTime);
    }
}
