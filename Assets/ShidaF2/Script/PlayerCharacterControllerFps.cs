using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerCharacterControllerFps : MonoBehaviour
{

    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterModel;
    [SerializeField] private SpaceshipRepair spaceShipRepair;


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

    // --- 加速度センサー（振る動作）用の変数を追加 ---
    private float lastAccelSum; 
    private float attackCooldown = 0.5f; // 連続攻撃を防ぐ待ち時間
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
        if (playerInput != null)
        {
            runaction = playerInput.actions["Dash"];
        }
    }

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();



    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
      
            if (!spaceShipRepair.isRepairing)
            {
            
                if (spaceShipRepair.repairButton.gameObject.activeInHierarchy && spaceShipRepair.repairButton.interactable)
                {
                
                    spaceShipRepair.repairButton.onClick.Invoke();
                }
            }
            else
            {
          
                spaceShipRepair.Repairing();
            }
        }
    }
   

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if (spaceShipRepair.isRepairing)
            {
                spaceShipRepair.Repairing(); 
                animator.SetTrigger("Attack1"); 

            }
            else
            {
                animator.SetTrigger("Attack1");
            }
        }
    }
    // private void Update()
    // {
    
    //     if (spaceShipRepair != null && spaceShipRepair.isRepairing)
    //     {
        
    //         if (animator != null) animator.SetFloat("Speed", 0f);
    //         return;
    //     }

    //     if (runaction != null && runaction.IsPressed())
    //     {
    //         isDashing = true;
    //     }
    //     else
    //     {
    //         isDashing = false;
    //     }

    //     UpdateLook();
    //     UpdateMovement();
    // }
private void Update()
    {
        if (spaceShipRepair != null && spaceShipRepair.isRepairing)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        var joysticks = UnityEngine.InputSystem.Joystick.all;
        if (joysticks.Count == 0) return;

        // --- 1つ目のデバイス (v2.2 / 移動・ダッシュ担当) ---
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
        // --- 2つ目のデバイス (v2.3 / 視点・振って攻撃担当) ---
    if (joysticks.Count >= 2)
    {
        lookInput = joysticks[1].stick.ReadValue();

        // ★ ここから攻撃判定（joysticks[1]、つまりv2.3だけを対象にする）
        float currentAccelSum = 0;
        
        // joysticks[1] (右手側) のコントロールだけをループで回す
        foreach (var control in joysticks[1].allControls)
        {
            // スティック(x, y)以外の「Rx, Ry, Rz」などの軸だけを計算
            if (control is UnityEngine.InputSystem.Controls.AxisControl axis && 
                control.name != "x" && control.name != "y")
            {
                currentAccelSum += Mathf.Abs(axis.ReadValue());
            }
        }

        // 加速度の変化（振りの強さ）を計算
        float deltaAccel = Mathf.Abs(currentAccelSum - lastAccelSum);

        // 0.8f を超えたら攻撃（v2.3を振ったときだけ実行される）
        if (deltaAccel > 1.0f && Time.time > lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack1");
            if (spaceShipRepair.isRepairing) spaceShipRepair.Repairing();
            
            lastAttackTime = Time.time;
            deltaAccel = 0f;
        }
        lastAccelSum = currentAccelSum;
    }

        UpdateLookL(); // 以前のUpdateLookLを使用
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

    // Time.deltaTimeを掛けることで、1フレームごとではなく「1秒間」の回転量になります。
    // これで動きが劇的にゆっくり、かつ滑らかになります。
    float yawInput = lookInput.x * padSensitivity * Time.deltaTime;
    float pitchInput = lookInput.y * padSensitivity * Time.deltaTime;

    // 左右の回転
    transform.Rotate(Vector3.up * yawInput);

    // 上下の回転（制限付き）
    pitch -= pitchInput;
    pitch = Mathf.Clamp(pitch, -90f, 90f);
    playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
}
    


    private void UpdateMovement()
    {
        // ★修理中は何もしない
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