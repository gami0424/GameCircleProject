using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    float defaultSpeed;
    Rigidbody rb;
    Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        defaultSpeed = moveSpeed;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveInput = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        velocity.z = moveInput.z * moveSpeed;
        rb.linearVelocity = velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SlowArea")) return;

        SlowArea area = other.GetComponent<SlowArea>();
        if (area != null)
        {
            moveSpeed = defaultSpeed * area.slowMultiplier;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("SlowArea")) return;

        moveSpeed = defaultSpeed;
    }
}


