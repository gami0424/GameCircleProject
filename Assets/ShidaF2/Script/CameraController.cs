using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("’Ç]Ý’è")]
    [SerializeField] private Transform targetPivot; 
    [SerializeField] private PlayerInput playerInput;

    [Header("Š´“xÝ’è")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float padSensitivity = 500f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (targetPivot != null)
        {
            // ‰ŠúˆÊ’u‚ð“¯Šú
            transform.position = targetPivot.position;
            yaw = targetPivot.eulerAngles.y;
        }
    }

    public void OnLook(InputValue value) => lookInput = value.Get<Vector2>();


    private void LateUpdate()
    {
        if (targetPivot == null) return;

  
        float sensitivity = (playerInput.currentControlScheme == "Gamepad") ? padSensitivity * Time.deltaTime : mouseSensitivity;
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -89f, 89f); 

   
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        transform.position = targetPivot.position;
    }
}