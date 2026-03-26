using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlowTrapController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DrillManChaseController drillMan;
    [SerializeField] private SlowTrapWall trapWall;
    [SerializeField] private PlayerCharacterControllerFps playerController;

    [Header("UI")]
    [SerializeField] private GameObject trapUIPanel;
    [SerializeField] private TextMeshProUGUI trapInfoText;
    [SerializeField] private Slider trapGauge;

    [Header("Settings")]
    [SerializeField] private float mashPower = 0.08f;
    [SerializeField] private float slowTimeScale = 0.25f;
    [SerializeField] private float playerSlowMultiplier = 0.6f;

    [SerializeField] private bool isPlayerNear = false;
    [SerializeField] private bool isTrapWorking = false;
    [SerializeField] private bool isGaugePhase = false;
    [SerializeField] private float currentGauge = 0f;

    public bool IsTrapWorking => isTrapWorking;

    private void Start()
    {
        if (trapUIPanel != null) trapUIPanel.SetActive(false);
        if (trapGauge != null) trapGauge.gameObject.SetActive(false);
       // gameObject.SetActive(false);
    }

    public void StartTrapMash()
    {
        if (!isPlayerNear || isTrapWorking || isGaugePhase) return;

        isGaugePhase = true;
        currentGauge = 0f;

        if (trapUIPanel != null) trapUIPanel.SetActive(true);
        if (trapGauge != null)
        {
            trapGauge.gameObject.SetActive(true);
            trapGauge.value = 0f;
        }

        if (trapInfoText != null)
        {
            trapInfoText.text = "ñWäQëïíuÇ≈êUÇÍÅIÅI";
        }

        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        if (playerController != null)
        {
            playerController.ApplySlow(playerSlowMultiplier);
        }

        if (drillMan != null)
        {
            drillMan.SetLowSpeedMode(true);
        }
    }

    public void MashTrap()
    {
        if (!isGaugePhase) return;

        currentGauge += mashPower;
        currentGauge = Mathf.Clamp01(currentGauge);

        if (trapGauge != null)
        {
            trapGauge.value = currentGauge;
        }

        if (trapWall != null)
        {
            trapWall.SetPreviewProgress(currentGauge);
        }

        if (currentGauge >= 1f)
        {
            FinishTrapMash();
        }
    }

    private void FinishTrapMash()
    {
        isGaugePhase = false;
        isTrapWorking = true;

        ResetTimeState();

        if (trapInfoText != null)
        {
            trapInfoText.text = "ñWäQê¨å˜ÅI";
        }

        if (trapGauge != null)
        {
            trapGauge.gameObject.SetActive(false);
        }

        if (trapWall != null)
        {
            trapWall.ActivateWall(drillMan);
        }

        Invoke(nameof(EndTrapUI), 1.0f);
    }

    private void EndTrapUI()
    {
        if (trapUIPanel != null)
        {
            trapUIPanel.SetActive(false);
        }

        isTrapWorking = false;
    }

    private void ResetTimeState()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (playerController != null)
        {
            playerController.ResetSpeed();
        }

        if (drillMan != null)
        {
            drillMan.SetLowSpeedMode(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = true;

        if (!isGaugePhase && !isTrapWorking && trapUIPanel != null && trapInfoText != null)
        {
            trapUIPanel.SetActive(true);
            trapInfoText.text = "ñWäQëïíuÇégÇ§Ç»ÇÁInteract";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = false;

        if (!isGaugePhase && trapUIPanel != null)
        {
            trapUIPanel.SetActive(false);
        }
    }
}