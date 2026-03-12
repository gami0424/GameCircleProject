using System.Collections.Generic;
using UnityEngine;

public class ScanTriggerCounter : MonoBehaviour
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

    private void OnTriggerExit(Collider other)
    {
        ScannablePart part = other.GetComponentInParent<ScannablePart>();
        if (part != null)
        {
            partsInRange.Remove(part);
        }
    }

    public void RemoveNullParts()
    {
        partsInRange.RemoveWhere(part => part == null);
    }
}