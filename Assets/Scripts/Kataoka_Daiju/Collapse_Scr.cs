using UnityEngine;
using UnityEngine.InputSystem;

public class Collapse_Scr : MonoBehaviour
{
    public float Speed = 10.0f;
    Rigidbody PLRigid;
    Keyboard keyboard = Keyboard.current;
    void Start()
    {
        PLRigid = GetComponent<Rigidbody>();
    }
    /*
        void Update()
        {
            // Keyboard.current が null でないかチェック（念のため）
            if (Keyboard.current != null)
            {
                // wasPressedThisFrame が旧 GetKeyDown に相当します
                if (keyboard.wKey.wasPressedThisFrame)
                {
                    Debug.Log("Wキーが押された（新システム方式）");
                }
                if (keyboard.wKey.isPressed)
                {

                }
            }
        }
    */
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("Ｗキーが押されている");
            PLRigid.AddForce(0, 0, Speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            PLRigid.AddForce(0, 0, -Speed, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            PLRigid.AddForce(Speed, 0, 0, ForceMode.Force);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collapse"))
        {
            Debug.Log("崩落エリアに侵入した");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collapse"))
        {
            Debug.Log("崩落フラグ踏んだ");
        }
    }
}
