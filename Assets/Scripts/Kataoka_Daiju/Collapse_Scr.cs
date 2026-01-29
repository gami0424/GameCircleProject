using UnityEngine;

public class Collapse_Scr : MonoBehaviour
{
    public float Speed = 10.0f;
    Rigidbody PLRigid;
    void Start()
    {
        PLRigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        PLRigid.AddForce(x * Speed, 0, z * Speed);
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
