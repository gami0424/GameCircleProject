using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int partCount = 0;
    private int partCount2 = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreText2;

    public int GetPartCount() => partCount;
    private void Start()
    {
        UpdateUI();
        UpdateUI2();

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

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Parts1Å~" + partCount;
        }


    }

    private void UpdateUI2()
    {
       

        if (scoreText2 != null)
        {
            scoreText2.text = "Parts2Å~" + partCount2;
        }

    }

    public void ConsumeParts(int amount)
    {
        partCount -= amount;
        UpdateUI();
    }

}
