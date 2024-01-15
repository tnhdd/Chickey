using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float distanceToPlayer;
    private Vector2 input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    private CameraRotation cameraRotation;
    [SerializeField] private CameraAngle cameraAngle;


    private void Awake()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
    }

    public void Look(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

    }
    void Update()
    {
        cameraRotation.yaw += input.x * mouseSensitivity.horizontal * Time.deltaTime;
        cameraRotation.pitch += input.y * mouseSensitivity.vertical * Time.deltaTime;
        cameraRotation.pitch = Mathf.Clamp(cameraRotation.pitch, cameraAngle.min, cameraAngle.max);
    }
    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(cameraRotation.pitch, cameraRotation.yaw, 0f);
        transform.position = target.position - transform.forward * distanceToPlayer;
    }
}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CameraRotation
{
    public float pitch;
    public float yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min, max;
}