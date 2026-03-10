using System.Collections.Generic;
using UnityEngine;

public class SScanTriggerCounter : MonoBehaviour
{
    private readonly HashSet<ScannablePart> partsInRange = new HashSet<ScannablePart>();


    public int PartCount => partsInRange.Count;

    private void OnTriggerEnter(Collider other)
    {
        ScannablePart part = other.GetComponentInParent<ScannablePart>();
        if (part != null)
        {
            partsInRange.Add(part);
        }
    }
}
