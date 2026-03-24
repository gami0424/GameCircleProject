using System.Collections.Generic;
using UnityEngine;

public class MeshRenderOffer : MonoBehaviour
{
    public List<MeshRenderer> mr;
    void Awake()
    {
        MROff(transform);
    }
    
    void MROff(Transform tra)
    {
        int child = tra.childCount;
        for (int i = 0; i < child; i++)
        {
            Debug.Log($"{tra.gameObject}:{child}:{i}");
            Transform chi = tra.GetChild(i);
            if (chi.childCount > 0) MROff(chi);
            if (chi.GetComponent<MeshRenderer>())
                chi.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
