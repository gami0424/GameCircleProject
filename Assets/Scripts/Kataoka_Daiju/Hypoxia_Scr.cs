using System.Collections;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;

public class Hypoxia_Scr : MonoBehaviour
{
    private float ElapsedTime = 0f;
    private bool isAreaIn = false;
    private GameObject player;
    public Player_Oxygen playerOxygen;
    public float OxygenDamage;
    public float DamageDelayTime = 3f;
    private bool isDamage;
    public Player_OxygenGauge playerOxygenGauge;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerOxygen = player.GetComponentInParent<Player_Oxygen>();
        playerOxygenGauge = player.GetComponentInParent<Player_OxygenGauge>();
    }
    void FixedUpdate()
    {
        if (player && playerOxygen && playerOxygenGauge == null) return;
        if (isAreaIn)
        {
            if (isDamage) return;
            StartCoroutine(DamageDelay());

            //ElapsedTime += Time.deltaTime;
            // 経過時間を表示(Fの横の数字まで小数点表示)
            //:Debug.Log("Time: " + ElapsedTime.ToString("F1"));
        }
    }
    IEnumerator DamageDelay()
    {
        isDamage = true;
        playerOxygen.TakeOxygenDamage(OxygenDamage);
        yield return new WaitForSeconds(DamageDelayTime);
        isDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAreaIn = true;
            Debug.Log("計測開始");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAreaIn = false;
            playerOxygen.CurrentOxygen = playerOxygen.MaxOxygen;
            playerOxygenGauge.TakeOxygenGaugeDamage(1f);
            /*Debug.Log("計測終了。滞在時間: " + ElapsedTime.ToString("F1"));
            ElapsedTime = 0f;*/
        }
    }
}