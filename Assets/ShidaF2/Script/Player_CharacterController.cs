using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_CharacterController : MonoBehaviour
{
    [Header("基本")]
    public CharacterController cc;
    public PlayerInput playerinput;
    private Rigidbody rd;


    [SerializeField] private Animator animator;

    [Header("カメラ")]
    public float yaw;
    public float Pitch;
    public Transform Cam;
    public Transform Cam2;

    public Transform CamP;
    public Transform CamY;
    public float sens;
    private float mouselooksensivity = 0.08f;
    private float padlooksensivity = 2.2f;
    public Vector3 camLook;
    public float camSpeed = 10;


    [Header("移動")]
    private float smoothrotaionvelocity;
    private float smoothrotaiontime = 0.12f;
    public float movespeed = 12f;



    [Header("入力")]
    private InputDevice lastinputdevice;
    private Vector2 moveinput;
    private Vector2 lookinput;

    public float gravity = -9.8f;

    private void Awake()
    {
        rd = GetComponent<Rigidbody>();

        if (playerinput != null && playerinput.actions != null)
        {

        }

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        cc = GetComponent<CharacterController>();

        if (Cam != null)
            yaw = Cam.transform.eulerAngles.y;
        Pitch = CamP != null ? CamP.localEulerAngles.x : 0f;

    }

    public void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookinput = value.Get<Vector2>();

    }
    private void OnEnable()
    {
        playerinput.onActionTriggered += OnActionTrigger;
    }

    private void Osable()
    {
        playerinput.onActionTriggered -= OnActionTrigger;
    }

    private void OnActionTrigger(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "Look")
        {
            lastinputdevice = ctx.control?.device;
        }
    }


    private void Update()
    {
        UpdateMovement();
        //PlMove();
    }

    private void UpdateMovement()
    {
        Vector3 move = Vector3.zero;

        if (moveinput.sqrMagnitude > 0.0001)
        {
            Vector3 inputDirection = new Vector3(moveinput.x, 0f, moveinput.y).normalized;
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
            + Cam.transform.rotation.eulerAngles.y;

            float rotation = Mathf.SmoothDamp(transform.rotation.eulerAngles.y, targetRotation, ref
            smoothrotaionvelocity, smoothrotaiontime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

            move = moveDir.normalized * (movespeed * Time.deltaTime);

            float verticalvelocity = gravity * Time.deltaTime;
            move.y = verticalvelocity;
            cc.Move(move);
        }
    }

    private void LateUpdate()
    {
        UpdateLook();
        // CameraMove();
    }

    private float GetSens()
    {
        if (lastinputdevice is Mouse) return mouselooksensivity;
        if (lastinputdevice is Gamepad) return padlooksensivity;
        return padlooksensivity;
    }
    private void UpdateLook()
    {
        sens = GetSens();
        yaw += lookinput.x * sens;
        Pitch -= lookinput.y * sens;
        Pitch = Mathf.Clamp(Pitch, -35, 70);

        CamP.rotation = Quaternion.Euler(Pitch, yaw, 0f);
    }



}
