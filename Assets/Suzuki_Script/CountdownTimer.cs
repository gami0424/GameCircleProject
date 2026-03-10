using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float time = 300f; // 制限時間（秒）
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameoverObj;


    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            time = 0;
            UpdateTimerText();
            GameoverText();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = minutes + ":" + seconds.ToString("00");
    }
    void GameoverText()
    {
        gameoverObj.SetActive(true);
    }
}
