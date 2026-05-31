using UnityEngine;

/// <summary>
/// Smooth chase camera that follows the player's car with dynamic FOV
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Distance Settings")]
    [SerializeField] private float distanceFromCar = 10f;
    [SerializeField] private float heightAboveCar = 4f;
    [SerializeField] private float forwardOffset = 3f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmoothness = 0.1f;
    [SerializeField] private float rotationSmoothness = 0.1f;

    [Header("Speed Effects")]
    [SerializeField] private float speedSensitivity = 0.02f;
    [SerializeField] private float minFOV = 50f;
    [SerializeField] private float maxFOV = 75f;
    [SerializeField] private float maxSpeed = 200f;

    [Header("References")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private CarController carController;
    [SerializeField] private Camera mainCamera;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (carTransform == null)
            carTransform = transform.parent;
        if (mainCamera == null)
            mainCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (carTransform == null)
            return;

        UpdateCameraPosition();
        UpdateCameraRotation();
        UpdateCameraFOV();
    }

    private void UpdateCameraPosition()
    {
        Vector3 carForward = carTransform.forward;
        Vector3 carUp = carTransform.up;

        Vector3 desiredPosition = carTransform.position 
            - carForward * distanceFromCar 
            + carUp * heightAboveCar 
            + carForward * forwardOffset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionSmoothness);
    }

    private void UpdateCameraRotation()
    {
        Vector3 lookAtPoint = carTransform.position + carTransform.forward * forwardOffset;
        Vector3 desiredDirection = (lookAtPoint - transform.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmoothness);
    }

    private void UpdateCameraFOV()
    {
        if (carController == null)
            return;

        float speedRatio = carController.CurrentSpeed / (maxSpeed * 3.6f);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, speedRatio);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * speedSensitivity);
    }
}