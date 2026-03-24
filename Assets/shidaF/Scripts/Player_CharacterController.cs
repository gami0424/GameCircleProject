using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_CharacterController_FPS : MonoBehaviour
{
    [Header("基本設定")]
    public CharacterController cc;
    public PlayerInput playerinput;


    [SerializeField] private Animator animator;

    [Header("カメラ設定")]

    public Transform playerCamera;

    [Header("感度設定")]
    public float mouseSensitivity = 10f;
    public float padSensitivity = 100f;

    [Header("移動設定")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;


    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private InputDevice lastInputDevice;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (cc == null)
            cc = GetComponent<CharacterController>();


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (playerinput != null)
        {
            playerinput.onActionTriggered += OnActionTrigger;
        }
    }

    private void OnDisable()
    {
        if (playerinput != null)
        {
            playerinput.onActionTriggered -= OnActionTrigger;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnActionTrigger(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "Look")
        {
            lastInputDevice = ctx.control?.device;
        }
    }

    private void Update()
    {

        moveLook();


        moveMovement();
    }


    private float GetSens()
    {
        if (lastInputDevice is Gamepad) return padSensitivity;
        return mouseSensitivity;
    }

    private void moveLook()
    {
        if (playerCamera == null) return;

        float sens = GetSens() * Time.deltaTime;


        float mouseX = lookInput.x * sens;
        float mouseY = lookInput.y * sens;


        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void moveMovement()
    {

        if (cc.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }


        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;


        cc.Move(move * moveSpeed * Time.deltaTime);


        verticalVelocity.y += gravity * Time.deltaTime;
        cc.Move(verticalVelocity * Time.deltaTime);
    }
}