using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterControllerFps : MonoBehaviour
{
    [Header("Mode Settings")]
    [SerializeField] private bool useSpecialDualController = true; 

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private SpaceshipRepair spaceShipRepair;
    [SerializeField] private Transform playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float padSensitivity = 500f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;

    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float pitch;
    private Vector3 verticalVelocity;
    private bool isDashing;
    private InputAction runaction;

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
        
        // Input Action Assetに "Dash" という名前のアクションがあることを確認してください
        if (playerInput != null) runaction = playerInput.actions["Dash"];
    }

    // --- ① 標準入力イベント（キーボード/通常パッド用） ---
    public void OnMove(InputValue value) { if (!useSpecialDualController) moveInput = value.Get<Vector2>(); }
    public void OnLook(InputValue value) { if (!useSpecialDualController) lookInput = value.Get<Vector2>(); }
    
    public void OnAttack(InputValue value)
    {
        if (useSpecialDualController) return; 
        if (value.isPressed) PerformAttack();
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed) HandleRepairInput();
    }

    private void Update()
    {
        // Pキーでモード切替
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            useSpecialDualController = !useSpecialDualController;
            // モード切替時に一旦入力をクリアする（暴走防止）
            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
            isDashing = false;
            Debug.Log($"モード切替完了: {(useSpecialDualController ? "ヌンチャク2台" : "キーボード/マウス")}");
        }

        if (spaceShipRepair != null && spaceShipRepair.isRepairing)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        // --- ② 入力ソースの確定 ---
        if (useSpecialDualController)
        {
            HandleDualJoystickInput();
        }
        else
        {
            // キーボードモード：Shiftキー（Dashアクション）が押されているかチェック
            if (runaction != null) 
            {
                isDashing = runaction.IsPressed();
            }
        }

        UpdateLookL(); 
        UpdateMovement();
    }

    private void HandleDualJoystickInput()
    {
        var joysticks = UnityEngine.InputSystem.Joystick.all;
        // ジョイスティックがない場合は何もしない（Update自体は止めない）
        if (joysticks.Count == 0) return;

        // --- 左手：移動・ダッシュ (v2.2) ---
        if (joysticks.Count >= 1)
        {
            moveInput = joysticks[0].stick.ReadValue();
            
            bool dashActive = false; 
            // 前回の修正通り "trigger" という名前のボタンだけを狙い撃ち
            var trigger = joysticks[0]["trigger"] as UnityEngine.InputSystem.Controls.ButtonControl;
            if (trigger != null && trigger.isPressed)
            {
                dashActive = true;
            }
            isDashing = dashActive;
        }

        // --- 右手：視点・振り攻撃 (v2.3) ---
        if (joysticks.Count >= 2)
        {
            lookInput = joysticks[1].stick.ReadValue();

            float currentSum = 0;
            foreach (var control in joysticks[1].allControls)
            {
                if (control is UnityEngine.InputSystem.Controls.AxisControl axis &&
                    control.name != "x" && control.name != "y")
                {
                    currentSum += Mathf.Abs(axis.ReadValue());
                }
            }

            float delta = Mathf.Abs(currentSum - lastAccelSum);
            if (delta > 1.2f && Time.time > lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = Time.time;
            }
            lastAccelSum = currentSum;
        }
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack1");
        if (spaceShipRepair.isRepairing) spaceShipRepair.Repairing();
    }

    private void UpdateLookL()
    {
        if (spaceShipRepair != null && spaceShipRepair.isRepairing) return;
        if (playerCamera == null) return;

        float yawInput, pitchInput;

        // キーボード/マウスモードかつコントロールスキームがGamepad以外（＝マウス）の場合
        if (!useSpecialDualController && (playerInput == null || playerInput.currentControlScheme != "Gamepad"))
        {
            yawInput = lookInput.x * mouseSensitivity;
            pitchInput = lookInput.y * mouseSensitivity;
        }
        else // ヌンチャクまたは通常パッド
        {
            yawInput = lookInput.x * padSensitivity * Time.deltaTime;
            pitchInput = lookInput.y * padSensitivity * Time.deltaTime;
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

        if (characterController.isGrounded && verticalVelocity.y < 0f) verticalVelocity.y = -2f;

        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (move.sqrMagnitude > 1f) move.Normalize();

        float speed = isDashing ? runSpeed : walkSpeed;
        characterController.Move(move * speed * Time.deltaTime);

        if (animator != null)
        {
            float animSpeed = move.magnitude > 0.1f ? speed : 0f;
            animator.SetFloat("Speed", animSpeed);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    private void HandleRepairInput()
    {
        if (!spaceShipRepair.isRepairing)
        {
            if (spaceShipRepair.repairButton.gameObject.activeInHierarchy && spaceShipRepair.repairButton.interactable)
                spaceShipRepair.repairButton.onClick.Invoke();
        }
        else spaceShipRepair.Repairing();
    }
}