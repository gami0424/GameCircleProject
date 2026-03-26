using UnityEngine;

public class FinalPartEventController : MonoBehaviour
{
    [SerializeField] private DrillManChaseController drillMan;
    [SerializeField] private Transform playerTransform;

    [Header("Close Side Roads")]
    [SerializeField] private GameObject[] blockWallsToEnable;

    [Header("Enable Trap Devices")]
    [SerializeField] private GameObject[] trapDevicesToEnable;

    private bool hasTriggered = false;

    public void TriggerFinalPartSequence()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (drillMan != null && playerTransform != null)
        {
            drillMan.ActivateChase(playerTransform);
        }

        for (int i = 0; i < blockWallsToEnable.Length; i++)
        {
            if (blockWallsToEnable[i] != null)
            {
                blockWallsToEnable[i].SetActive(true);
            }
        }

        for (int i = 0; i < trapDevicesToEnable.Length; i++)
        {
            if (trapDevicesToEnable[i] != null)
            {
                trapDevicesToEnable[i].SetActive(true);
            }
        }
    }
}