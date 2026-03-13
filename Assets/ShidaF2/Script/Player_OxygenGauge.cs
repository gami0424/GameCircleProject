using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Player_OxygenGauge : MonoBehaviour
{
    [SerializeField] private Image OxygenGaugeImage;

    public GameObject OxygenBar;
    public float dur = 0.5f;
    public float strength = 20f;
    public int vibrate = 100;
    public float FirstBarRate = 1f;

    void Start()
    {
        SetGauge(FirstBarRate);
    }

    private void SetGauge(float SetRate)
    {
        OxygenGaugeImage.DOKill();
        OxygenGaugeImage.DOKill();
        SetRate = Mathf.Clamp01(SetRate);

        OxygenGaugeImage.DOFillAmount(SetRate, dur).OnComplete(() =>
        {
            OxygenGaugeImage.DOFillAmount(SetRate, dur * 0.5f).SetDelay(0.5f);
        });
        if (SetRate == FirstBarRate) return;
        OxygenBar.transform.DOShakePosition(dur * 0.5f, strength, vibrate);
        //curBarRate = SetRate;
    }


    public void TakeOxygenGaugeDamage(float Rate)
    {
        SetGauge(Rate);
    }



    void Update()
    {

    }
}
