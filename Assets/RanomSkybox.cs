using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanomSkybox : MonoBehaviour
{
    [SerializeField] Material[] skyboxMaterials;
    // Start is called before the first frame update
    void Start()
    {
        // Check if skyboxMaterials array is not empty
        if (skyboxMaterials != null && skyboxMaterials.Length > 0)
        {
            // Randomly select an index from the skyboxMaterials array
            int randomIndex = Random.Range(0, skyboxMaterials.Length);

            // Apply the randomly selected skybox material to the scene's skybox
            RenderSettings.skybox = skyboxMaterials[randomIndex];
        }
        else
        {
            Debug.LogError("Skybox materials array is empty or not assigned!");
        }
    }

    
}
