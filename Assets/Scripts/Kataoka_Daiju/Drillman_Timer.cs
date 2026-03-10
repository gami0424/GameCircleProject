using UnityEngine;

public class Drillman_Timer : MonoBehaviour
{
  [SerializeField ]private float LimitTime = 5f; //1f = 1秒

   private void Update()
    {
        if (LimitTime <= 0)
        {
            Debug.Log("Time Over");
            return;
        }
        LimitTime -= Time.deltaTime;  //初期設定では　Time.deltaTime == 0.02
        Debug.Log("残り時間: " + LimitTime.ToString("F1"));  //Fの横の数字までの桁の小数点を表示
    }
}
