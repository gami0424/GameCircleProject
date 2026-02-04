using UnityEngine;

public class Hypoxia_Scr : MonoBehaviour
{
    private float ElapsedTime = 0f;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            ElapsedTime += Time.deltaTime;
            // ログに経過した時間を表示（小数点1桁まで）
            Debug.Log("Time: " + ElapsedTime.ToString("F1"));
        }
    }

    // エリアに入った時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーに "Player" タグがついている場合
        {
            isRunning = true;
            Debug.Log("計測開始");
        }
    }

    // エリアから出た時
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isRunning = false;
            Debug.Log("計測終了。滞在時間: " + ElapsedTime.ToString("F2"));
            ElapsedTime = 0f;
        }
    }
}