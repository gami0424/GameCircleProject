using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterControllerFps : MonoBehaviour
{
    [Header("基本設定")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;

    [Header("カメラ設定")]
    [SerializeField] private Transform playerCamera;

    [Header("感度設定")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float padSensitivity = 300f;

    [Header("移動設定")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;

    private float defaultWalkSpeed;
    private float defaultRunSpeed;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float pitch;
    private InputDevice lastLookInputDevice;
    private Vector3 verticalVelocity;
    private bool isDashing;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnDash(InputValue value)
    {
        isDashing = value.isPressed;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Look")
        {
            lastLookInputDevice = context.control?.device;
        }
    }

    private void Update()
    {
        UpdateLook();
        UpdateMovement();
    }

   

    private void UpdateLook()
    {
        if (playerCamera == null)
        {
            return;
        }

        float yawInput;
        float pitchInput;

        if (lastLookInputDevice is Gamepad)
        {
            yawInput = lookInput.x * padSensitivity * Time.deltaTime;
            pitchInput = lookInput.y * padSensitivity * Time.deltaTime;
        }
        else
        {
            yawInput = lookInput.x * mouseSensitivity;
            pitchInput = lookInput.y * mouseSensitivity;
        }

        pitch -= pitchInput;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * yawInput);
    }

    private void UpdateMovement()
    {
        if (characterController == null)
        {
            return;
        }

        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        bool hasMoveInput = moveInput.magnitude > 0.1f;
        float currentSpeed = isDashing ? runSpeed : walkSpeed;

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (animator != null)
        {
            float animatorSpeed = 0f;

            if (hasMoveInput)
            {
                animatorSpeed = isDashing ? runSpeed : walkSpeed;
            }

            animator.SetFloat("Speed", animatorSpeed);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }


    public void ApplySlow(float slowMultiplier)
    {
        walkSpeed = defaultWalkSpeed * slowMultiplier;
        runSpeed = defaultRunSpeed * slowMultiplier;
    }

    public void ResetSpeed()
    {
        walkSpeed = defaultWalkSpeed;
        runSpeed = defaultRunSpeed;
    }
}