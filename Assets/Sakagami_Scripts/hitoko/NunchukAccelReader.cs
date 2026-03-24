using UnityEngine;
using UnityEngine.InputSystem;

public class NunchukAccelReader : MonoBehaviour
{
    [SerializeField] private InputActionReference accelX;

    [SerializeField] private int Value = 1;
    // [SerializeField] private InputActionReference accelY;
    // [SerializeField] private InputActionReference accelZ;

    private void OnEnable()
    {
        accelX.action.Enable();
        // accelY.action.Enable();
        // accelZ.action.Enable();
    }

    private void OnDisable()
    {
        accelX.action.Disable();
        // accelY.action.Disable();
        // accelZ.action.Disable();
    }

    private void Update()
    {
        float x = accelX.action.ReadValue<float>();
        x=x*Value;
        // float y = accelY.action.ReadValue<float>();
        // float z = accelZ.action.ReadValue<float>();

       Debug.Log(x);
    }
}