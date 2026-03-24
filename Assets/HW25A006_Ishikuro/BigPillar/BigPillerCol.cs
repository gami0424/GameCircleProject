using UnityEngine;

public class BigPillerCol : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit {other.gameObject.name}!!");
    }
}
