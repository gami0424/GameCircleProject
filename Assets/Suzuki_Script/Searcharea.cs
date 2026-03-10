using UnityEngine;

public class Searcharea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        item_parts target = other.GetComponent<item_parts>();

        if (target != null)
        {
            target.ShowOutline();
        }
    }

    void OnTriggerExit(Collider other)
    {
        item_parts target = other.GetComponent<item_parts>();

        if (target != null)
        {
            target.HideOutline();
        }
    }
}
