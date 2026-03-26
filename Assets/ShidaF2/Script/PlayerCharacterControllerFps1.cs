using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterControllerFps1 : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterModel;
    [SerializeField] private SpaceshipRepair spaceShipRepair;
    [SerializeField] private SlowTrapController slowTrapController;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float padSensitivity = 500f;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float gravity = -9.81f;

    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float pitch;
    private Vector3 verticalVelocity;
    private bool isDashing;
    private InputAction runaction;

    // êUÇÈìÆçÏóp
    private float lastAccelSum;
    private float attackCooldown = 0.5f;
    private float lastAttackTime;

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        defaultWalkSpeed = walkSpeed;
        defaultRunSpeed = runSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInput != null && playerInput.actions != null)
        {
            runaction = playerInput.actions["Dash"];
        }
    }

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        Debug.Log("OnInteract åƒÇŒÇÍÇΩ");

        // á@ ÉgÉâÉbÉväJén
        if (slowTrapController != null)
        {
            slowTrapController.StartTrapMash();
        }

        // áA âFíàëDèCóùäJén or èCóùêiçs
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

        // ÉgÉâÉbÉvÇÃÉQÅ[ÉWêiçs
        if (slowTrapController != null)
        {
            slowTrapController.MashTrap();
        }

        // âFíàëDèCóùÇÃÉQÅ[ÉWêiçs
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
        // âFíàëDèCóùíÜÇÕí‚é~
        if (spaceShipRepair != null && spaceShipRepair.isRepairing)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        var joysticks = UnityEngine.InputSystem.Joystick.all;
        if (joysticks.Count == 0) return;

        // ç∂éËë§: à⁄ìÆ / É_ÉbÉVÉÖ
        if (joysticks.Count >= 1)
        {
            moveInput = joysticks[0].stick.ReadValue();

            bool dashButtonPressed = false;
            foreach (var control in joysticks[0].allControls)
            {
                if (control.name.Contains("trigger") || control.name.Contains("button"))
                {
                    var button = control as UnityEngine.InputSystem.Controls.ButtonControl;
                    if (button != null && button.isPressed)
                    {
                        dashButtonPressed = true;
                        break;
                    }
                }
            }
            isDashing = dashButtonPressed;
        }

        // âEéËë§: éãì_ / êUÇËçUåÇ
        if (joysticks.Count >= 2)
        {
            lookInput = joysticks[1].stick.ReadValue();

            float currentAccelSum = 0f;

            foreach (var control in joysticks[1].allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.AxisControl axis &&
                    control.name != "x" && control.name != "y")
                {
                    currentAccelSum += Mathf.Abs(axis.ReadValue());
                }
            }

            float deltaAccel = Mathf.Abs(currentAccelSum - lastAccelSum);

            if (deltaAccel > 1.0f && Time.time > lastAttackTime + attackCooldown)
            {
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

                lastAttackTime = Time.time;
            }

            lastAccelSum = currentAccelSum;
        }

        UpdateLookL();
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

    private void UpdateLookL()
    {
        if (spaceShipRepair != null && spaceShipRepair.isRepairing) return;
        if (playerCamera == null) return;

        float yawInput = lookInput.x * padSensitivity * Time.deltaTime;
        float pitchInput = lookInput.y * padSensitivity * Time.deltaTime;

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