using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int partCount = 0;
    private int partCount2 = 0;
    private int finalPartCount = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreText2;
    [SerializeField] private TextMeshProUGUI finalPartText;
    [SerializeField] private TextMeshProUGUI messageText;

    public int GetPartCount() => partCount;
    public int GetPartCount2() => partCount2;
    public int GetFinalPartCount() => finalPartCount;
    public bool HasFinalPart() => finalPartCount > 0;

    private void Start()
    {
        UpdateUI();
        UpdateUI2();
        UpdateFinalPartUI();

        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);//
        }
    }

    

    public void AddPart(int amount)
    {
        partCount += amount;
        UpdateUI();
    }

    public void AddPart2(int amount)
    {
        partCount2 += amount;
        UpdateUI2();
    }

    public void AddFinalPart(int amount)
    {
        finalPartCount += amount;
        if (finalPartCount < 0) finalPartCount = 0;
        UpdateFinalPartUI();
    }

    public void ConsumeParts(int amount)
    {
        partCount -= amount;
        if (partCount < 0) partCount = 0;
        UpdateUI();
    }

    public void ConsumeFinalPart(int amount)
    {
        finalPartCount -= amount;
        if (finalPartCount < 0) finalPartCount = 0;
        UpdateFinalPartUI();
    }

    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Parts1~" + partCount;
        }
    }

    private void UpdateUI2()
    {
        if (scoreText2 != null)
        {
            scoreText2.text = "Parts2~" + partCount2;
        }
    }

    private void UpdateFinalPartUI()
    {
        if (finalPartText != null)
        {
            finalPartText.text = "FinalPart~" + finalPartCount;
        }
    }
}