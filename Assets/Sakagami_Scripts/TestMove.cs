using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Wキーを押している間、前に進む
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}

