using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SpaceshipRepair : MonoBehaviour
{
    [SerializeField] private int requiredParts = 5;
    [SerializeField] private float mashPower = 0.05f;
    [SerializeField] private GameObject brokenMesh;
    [SerializeField] private GameObject fixedMesh;
    [SerializeField] private GameObject repairUIPanel;
    [SerializeField] private Image imagePanel;
    [SerializeField] private TextMeshProUGUI infoText;
   public Button repairButton;
    [SerializeField] private Slider repairGauge;

    private PlayerInventory playerInv;
    private bool isPlayerNear = false;
  public bool isRepairing = false;
    private bool isFinished = false;
    private float currentProgress = 0f;

    void Start()
    {
        fixedMesh.SetActive(false);
        repairUIPanel.SetActive(false);
        repairGauge.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFinished || !isPlayerNear || isRepairing) return;

        if (playerInv.GetPartCount() >= requiredParts)
        {
            infoText.text = "パーツが揃った！修理開始！";
            repairButton.interactable = true;
        }
        else
        {
            infoText.text = $"パーツ不足： {playerInv.GetPartCount()} / {requiredParts}";
            repairButton.interactable = false;
        }
    }

    public void Repairing()
    {
        if (isRepairing && !isFinished)
        {
            currentProgress += mashPower;
            repairGauge.value = currentProgress;
            if (currentProgress >= 1f)
            {
                FinishRepair();
            }
        }
    }

    public void StartRepair()
    {
        if (isFinished) return;

        isRepairing = true;
        playerInv.ConsumeParts(requiredParts);

        repairButton.gameObject.SetActive(false);
        repairGauge.gameObject.SetActive(true);
        infoText.text = "ボタン連打で修理しろ！";
        currentProgress = 0f;
        repairGauge.value = 0f;
    }

    void FinishRepair()
    {
        isRepairing = false;
        isFinished = true;
        repairUIPanel.SetActive(false);

        brokenMesh.SetActive(false);
        fixedMesh.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFinished) return;

        if (other.CompareTag("Player"))
        {
            playerInv = other.GetComponent<PlayerInventory>();
            isPlayerNear = true;

            if (playerInv.GetPartCount() >= requiredParts)
            {
                imagePanel.color = new Color(0.6f, 1.0f, 0.6f, 0.5f);
            }
            else
            {
                imagePanel.color = new Color(1.0f, 0.6f, 0.6f, 0.5f);
            }

            repairUIPanel.SetActive(true);
            if (repairButton != null)
            {
                repairButton.Select();
                EventSystem.current.SetSelectedGameObject(repairButton.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (!isRepairing) repairUIPanel.SetActive(false);
        }
    }
}