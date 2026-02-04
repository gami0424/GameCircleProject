using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Player_HpGauge : MonoBehaviour
{
    [SerializeField] private Image HpGaugeImage;
    [SerializeField] private Image BurnGaugeImage;
    public GameObject HpBar;
    public float dur = 0.5f;
    public float strength = 20f;
    public int vibrate = 100;
    public float curBarRate = 1f;
    void Start()
    {
        
    }

    private void SetGauge(float SetRate)
    {
        HpGaugeImage.DOKill();
        BurnGaugeImage.DOKill();
        SetRate = Mathf.Clamp01(SetRate);

        HpGaugeImage.DOFillAmount(SetRate, dur).OnComplete(() =>
        {
            BurnGaugeImage.DOFillAmount(SetRate, dur * 0.5f).SetDelay(0.5f);
        });

        HpBar.transform.DOShakePosition(dur * 0.5f, strength, vibrate);
    }
    void Update()
    {
        
    }
}
