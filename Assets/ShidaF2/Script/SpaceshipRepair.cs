using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SpaceshipRepair : MonoBehaviour
{
    [Header("Need Parts")]
    [SerializeField] private int requiredParts = 5;

    [Header("Repair Setting")]
    [SerializeField] private float mashPower = 0.05f;

    [Header("Machine")]
    [SerializeField] private GameObject brokenMesh;
    [SerializeField] private GameObject fixedMesh;

    [Header("UI")]
    [SerializeField] private GameObject repairUIPanel;
    [SerializeField] private Image imagePanel;
    [SerializeField] private TextMeshProUGUI infoText;
public Button repairButton;
    [SerializeField] private Slider repairGauge;
    [SerializeField] private Button escapeButton;

    [Header("Scene")]
    [SerializeField] private string resultSceneName = "ResultScene";

    private PlayerInventory playerInv;
    [SerializeField] private bool isPlayerNear = false;
    public bool isRepairing = false;
    private bool isFinished = false;
    private float currentProgress = 0f;

    void Start()
    {
        if (fixedMesh != null) fixedMesh.SetActive(false);
        if (repairUIPanel != null) repairUIPanel.SetActive(false);
        if (repairGauge != null) repairGauge.gameObject.SetActive(false);
        if (escapeButton != null) escapeButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFinished || !isPlayerNear || isRepairing || playerInv == null) return;

        bool hasEnoughNormalParts = playerInv.GetPartCount() >= requiredParts;
        bool hasFinalPart = playerInv.HasFinalPart();
     
        if (hasEnoughNormalParts && !hasFinalPart)
        {
            infoText.text = "最後のパーツを取りに行こう！！";
            repairButton.interactable = false;

            if (imagePanel != null)
            {
                imagePanel.color = new Color(1.0f, 0.9f, 0.4f, 0.5f);
            }

            playerInv.ShowMessage("最後のパーツを取りに行こう！！");
        }
        else if (hasEnoughNormalParts && hasFinalPart)
        {
            infoText.text = "最後のパーツを手に入れた！修理しよう！！";
            repairButton.interactable = true;

            if (imagePanel != null)
            {
                imagePanel.color = new Color(0.6f, 1.0f, 0.6f, 0.5f);
            }
        }
        else
        {
            
            infoText.text = $"パーツ不足： {playerInv.GetPartCount()} / {requiredParts}";
            repairButton.interactable = false;

            if (imagePanel != null)
            {
                imagePanel.color = new Color(1.0f, 0.6f, 0.6f, 0.5f);
            }
        }
    }

    public void StartRepair()
    {
        if (isFinished) return;
        if (playerInv == null) return;
        if (playerInv.GetPartCount() < requiredParts) return;
        if (!playerInv.HasFinalPart()) return;

        isRepairing = true;

        playerInv.ConsumeParts(requiredParts);
        playerInv.ConsumeFinalPart(1);

        repairButton.gameObject.SetActive(false);
        repairGauge.gameObject.SetActive(true);

        infoText.text = "急いで修理して脱出するために振れ！！";
        playerInv.ShowMessage("急いで修理して脱出するために振れ！！");

        currentProgress = 0f;
        repairGauge.value = 0f;
    }

    public void Repairing()
    {
        if (!isRepairing || isFinished) return;

        currentProgress += mashPower;
        repairGauge.value = currentProgress;

        if (currentProgress >= 1f)
        {
            FinishRepair();
        }
    }

    private void FinishRepair()
    {
        isRepairing = false;
        isFinished = true;

        repairGauge.gameObject.SetActive(false);

        if (brokenMesh != null) brokenMesh.SetActive(false);
        if (fixedMesh != null) fixedMesh.SetActive(true);

        infoText.text = "脱出！！";
        if (playerInv != null)
        {
            playerInv.ShowMessage("脱出！！");
        }

        if (escapeButton != null)
        {
            escapeButton.gameObject.SetActive(true);
            escapeButton.interactable = true;
            escapeButton.Select();
            EventSystem.current.SetSelectedGameObject(escapeButton.gameObject);
        }
    }

    public void Escape()
    {
        SceneManager.LoadScene(resultSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFinished) return;

        if (other.CompareTag("Player"))
        {
            playerInv = other.GetComponent<PlayerInventory>();
            isPlayerNear = true;

            repairUIPanel.SetActive(true);
            infoText.gameObject.SetActive(true);
            if (repairButton != null)
            {
                repairButton.gameObject.SetActive(true);
                repairButton.Select();
                EventSystem.current.SetSelectedGameObject(repairButton.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = false;
        infoText.gameObject.SetActive(false);
        if (!isRepairing && !isFinished)
        {
            repairUIPanel.SetActive(false);
        }
    }
}