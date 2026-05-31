using UnityEngine;

/// <summary>
/// Handles cross-platform input (mobile accelerometer & PC keyboard)
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Mobile Settings")]
    [SerializeField] private float tiltSensitivity = 1.5f;
    [SerializeField] private float tiltDeadzone = 0.1f;

    [Header("PC Settings")]
    [SerializeField] private float keyboardSensitivity = 2f;

    private float currentSteerInput = 0f;
    private float currentAccelerationInput = 0f;
    private float currentBrakeInput = 0f;
    private bool nitroActive = false;
    private bool isMobileInput = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        #if UNITY_ANDROID || UNITY_IOS
            isMobileInput = true;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        #else
            isMobileInput = false;
        #endif
    }

    private void Update()
    {
        if (isMobileInput)
            HandleMobileInput();
        else
            HandlePCInput();
    }

    private void HandleMobileInput()
    {
        float tiltX = Input.acceleration.x;
        if (Mathf.Abs(tiltX) < tiltDeadzone)
            tiltX = 0f;

        currentSteerInput = Mathf.Clamp(tiltX * tiltSensitivity, -1f, 1f);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.y > Screen.height * 0.5f)
            {
                currentAccelerationInput = 1f;
                currentBrakeInput = 0f;
            }
            else
            {
                currentAccelerationInput = 0f;
                currentBrakeInput = 1f;
            }
        }
        else
        {
            currentAccelerationInput = 0f;
            currentBrakeInput = 0f;
        }

        nitroActive = Input.touchCount == 2;
    }

    private void HandlePCInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        currentSteerInput = Mathf.Clamp(horizontalInput * keyboardSensitivity, -1f, 1f);

        currentAccelerationInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
        currentBrakeInput = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Space)) ? 1f : 0f;
        nitroActive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    public float GetSteerInput() => currentSteerInput;
    public float GetAccelerationInput() => currentAccelerationInput;
    public float GetBrakeInput() => currentBrakeInput;
    public bool GetNitroInput() => nitroActive;
    public bool IsMobileInput() => isMobileInput;
}