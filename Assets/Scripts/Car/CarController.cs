using UnityEngine;

/// <summary>
/// Main car controller with realistic physics, acceleration, braking, and steering
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("Engine Settings")]
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float brakePower = 80f;
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float steerSensitivity = 2f;

    [Header("Physics")]
    [SerializeField] private float dragCoefficient = 0.1f;
    [SerializeField] private float weight = 1500f;
    [SerializeField] private float centerOfMassHeight = 0.3f;

    [Header("Wheels")]
    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;

    [Header("Wheel Models")]
    [SerializeField] private Transform frontLeftWheelModel;
    [SerializeField] private Transform frontRightWheelModel;
    [SerializeField] private Transform rearLeftWheelModel;
    [SerializeField] private Transform rearRightWheelModel;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float currentSteerAngle = 0f;
    private float currentMotorForce = 0f;
    private float currentBrakeForce = 0f;
    private bool isGrounded = true;

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetupPhysics();
    }

    private void SetupPhysics()
    {
        rb.centerOfMass = new Vector3(0, centerOfMassHeight, 0);
        ConfigureWheel(frontLeftWheel);
        ConfigureWheel(frontRightWheel);
        ConfigureWheel(rearLeftWheel);
        ConfigureWheel(rearRightWheel);
    }

    private void ConfigureWheel(WheelCollider wheel)
    {
        wheel.mass = weight / 4f;
        wheel.suspensionDistance = 0.3f;

        JointSpring suspension = wheel.suspensionSpring;
        suspension.spring = 35000f;
        suspension.damper = 4000f;
        suspension.targetPosition = 0.5f;
        wheel.suspensionSpring = suspension;

        WheelFrictionCurve frictionCurve = wheel.forwardFriction;
        frictionCurve.stiffness = 1f;
        frictionCurve.extremumSlip = 0.4f;
        frictionCurve.extremumValue = 1f;
        wheel.forwardFriction = frictionCurve;

        WheelFrictionCurve sideFriction = wheel.sidewaysFriction;
        sideFriction.stiffness = 1f;
        sideFriction.extremumSlip = 0.25f;
        sideFriction.extremumValue = 1f;
        wheel.sidewaysFriction = sideFriction;
    }

    private void Update()
    {
        float motorInput = InputManager.Instance.GetAccelerationInput();
        float brakeInput = InputManager.Instance.GetBrakeInput();
        float steerInput = InputManager.Instance.GetSteerInput();
        bool isNitroActive = InputManager.Instance.GetNitroInput();

        HandleSteering(steerInput);
        HandleThrottle(motorInput, brakeInput, isNitroActive);
        UpdateWheelRotation();
    }

    private void FixedUpdate()
    {
        ApplyMotor();
        ApplyBrake();
        ApplySteer();

        isGrounded = frontLeftWheel.isGrounded && frontRightWheel.isGrounded && 
                    rearLeftWheel.isGrounded && rearRightWheel.isGrounded;

        ApplyAirResistance();
        currentSpeed = rb.velocity.magnitude * 3.6f;
    }

    private void HandleSteering(float steerInput)
    {
        float speedFactor = 1f - (currentSpeed / (maxSpeed * 3.6f)) * 0.5f;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, steerInput * maxSteerAngle * speedFactor, Time.deltaTime * steerSensitivity);
    }

    private void HandleThrottle(float motorInput, float brakeInput, bool isNitro)
    {
        if (motorInput > 0)
        {
            currentMotorForce = motorInput * acceleration * 1000f;
            if (isNitro && currentSpeed < maxSpeed * 3.6f * 0.8f)
                currentMotorForce *= 1.5f;
            currentBrakeForce = 0f;
        }
        else if (brakeInput > 0)
        {
            currentMotorForce = 0f;
            currentBrakeForce = brakeInput * brakePower * 1000f;
        }
        else
        {
            currentMotorForce = 0f;
            currentBrakeForce = 0f;
        }
    }

    private void ApplyMotor()
    {
        frontLeftWheel.motorTorque = currentMotorForce;
        frontRightWheel.motorTorque = currentMotorForce;
    }

    private void ApplyBrake()
    {
        frontLeftWheel.brakeTorque = currentBrakeForce * 0.6f;
        frontRightWheel.brakeTorque = currentBrakeForce * 0.6f;
        rearLeftWheel.brakeTorque = currentBrakeForce * 0.4f;
        rearRightWheel.brakeTorque = currentBrakeForce * 0.4f;
    }

    private void ApplySteer()
    {
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }

    private void UpdateWheelRotation()
    {
        UpdateWheelModel(frontLeftWheel, frontLeftWheelModel);
        UpdateWheelModel(frontRightWheel, frontRightWheelModel);
        UpdateWheelModel(rearLeftWheel, rearLeftWheelModel);
        UpdateWheelModel(rearRightWheel, rearRightWheelModel);
    }

    private void UpdateWheelModel(WheelCollider wheelCollider, Transform wheelModel)
    {
        if (wheelModel == null) return;
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelModel.position = position;
        wheelModel.rotation = rotation;
    }

    private void ApplyAirResistance()
    {
        float speed = rb.velocity.magnitude;
        Vector3 dragForce = -rb.velocity.normalized * dragCoefficient * speed * speed;
        rb.AddForce(dragForce, ForceMode.Acceleration);
    }

    public void ResetCar(Transform resetPosition)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = resetPosition.position;
        transform.rotation = resetPosition.rotation;
    }
}