using UnityEngine;
using TMPro;

public class Result_time : MonoBehaviour
{
    [SerializeField] private float time = 0f;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameoverObj;
    [SerializeField] private GameObject ResultObj;
    [SerializeField] private GameObject ShakeCount;

    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        time += Time.deltaTime;

        if (time >= 300f)
        {
            GameOver();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = minutes + ":" + seconds.ToString("00");
    }
    void GameOver()
    {
        isRunning = false;
        ResultObj.SetActive(false);
        gameoverObj.SetActive(true);
    }

    public void StopTimer()
    {
        isRunning = false;
        UpdateTimerText();
        ResultText();
    }

    void ResultText()
    {
        gameoverObj.SetActive(false);
        ResultObj.SetActive(true);
    }
}