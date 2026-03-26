using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterControllerFps : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private SpaceshipRepair spaceShipRepair;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private SlowTrapController slowTrapController;

    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float padSensitivity = 500f;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private InputActionReference tiltXAction;
    [SerializeField] private InputActionReference tiltYAction;
    [SerializeField] private InputActionReference tiltZAction;

    [SerializeField] private float threshold = 1.0f;
    [SerializeField] private float cooldown = 0.35f;

    private float lastTriggerTime = -999f;

    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float pitch;
    private Vector3 verticalVelocity;
    private bool isDashing;
    private InputAction runaction;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInput != null)
        {
            runaction = playerInput.actions["Dash"];
        }
    }

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    private void OnEnable()
    {
        if (tiltXAction != null) tiltXAction.action.Enable();
        if (tiltYAction != null) tiltYAction.action.Enable();
        if (tiltZAction != null) tiltZAction.action.Enable();
    }

    private void OnDisable()
    {
        if (tiltXAction != null) tiltXAction.action.Disable();
        if (tiltYAction != null) tiltYAction.action.Disable();
        if (tiltZAction != null) tiltZAction.action.Disable();
    }
    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        Debug.Log("InteractâüÇµÇΩ");

        if (slowTrapController != null)
        {
            Debug.Log("slowTrapControllerÇ†ÇË: " + slowTrapController.name);
            slowTrapController.StartTrapMash();
        }
        else
        {
            Debug.LogError("slowTrapControllerÇ™ñ¢ê›íË");
        }

        if (spaceShipRepair == null) return;

        if (!spaceShipRepair.isRepairing)
        {
            if (spaceShipRepair.repairButton != null &&
                spaceShipRepair.repairButton.gameObject.activeInHierarchy &&
                spaceShipRepair.repairButton.interactable)
            {
                spaceShipRepair.repairButton.onClick.Invoke();
            }
        }
        else
        {
            spaceShipRepair.Repairing();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed) return;

        if (slowTrapController != null)
        {
            slowTrapController.MashTrap();
        }

        if (spaceShipRepair != null && spaceShipRepair.isRepairing)
        {
            spaceShipRepair.Repairing();
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack1");
        }
    }
    private void Update()
    {
        if (spaceShipRepair != null && spaceShipRepair.isRepairing)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        isDashing = runaction != null && runaction.IsPressed();

        UpdateLook();
        UpdateMovement();
    }

    private void UpdateLook()
    {

        if (spaceShipRepair != null && spaceShipRepair.isRepairing) return;
        if (playerCamera == null || playerInput == null) return;

        float yawInput;
        float pitchInput;

        if (playerInput.currentControlScheme == "Gamepad")
        {
            yawInput = lookInput.x * padSensitivity * Time.deltaTime;
            pitchInput = lookInput.y * padSensitivity * Time.deltaTime;
        }
        else
        {
            yawInput = lookInput.x * mouseSensitivity;
            pitchInput = lookInput.y * mouseSensitivity;
        }

        transform.Rotate(Vector3.up * yawInput);

        pitch -= pitchInput;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void UpdateMovement()
    {
        if (spaceShipRepair != null && spaceShipRepair.isRepairing) return;
        if (characterController == null) return;

        if (characterController.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        float currentSpeed = isDashing ? runSpeed : walkSpeed;
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (animator != null)
        {
            bool hasMoveInput = moveInput.magnitude > 0.1f;
            float animatorSpeed = hasMoveInput ? (isDashing ? runSpeed : walkSpeed) : 0f;
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