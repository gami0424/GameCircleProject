using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("設定")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 150f;
    public Transform cameraBox; // ここにCameraBoxをドラッグする

    private float verticalRotation = 0f;

    void Start()
    {
        // マウスをロック（コントローラー操作でも推奨）
        Cursor.lockState = CursorLockMode.Locked;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        Debug.Log($"LookX: {Input.GetAxisRaw("Joystick Axis 4")} | LookY: {Input.GetAxisRaw("Joystick Axis 5")}");





        // --- 1. 移動 (左スティック) ---
        float mX = Input.GetAxisRaw("Horizontal");
        float mY = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(mX) > 0.1f || Mathf.Abs(mY) > 0.1f)
        {
            // Player本体の向きを基準に移動
            Vector3 moveDir = (transform.forward * mY) + (transform.right * mX);
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
        }


        // --- 2. 視点移動 (右スティック) ---
        // --- 右スティック ---
        float lookX = Input.GetAxisRaw("Joystick Axis 4"); // 上-1 下 1
        float lookY = -Input.GetAxisRaw("Joystick Axis 5"); // ??


        // 左右回転
        if (Mathf.Abs(lookY) > 0.1f)
        {
            transform.Rotate(Vector3.up * lookY * lookSensitivity * Time.deltaTime * -1);
        }


        // 上下回転
        if (cameraBox != null && Mathf.Abs(lookX) > 0.1f)
        {
            verticalRotation -= lookX * lookSensitivity * Time.deltaTime * -1;
            verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
            cameraBox.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

    }
}

