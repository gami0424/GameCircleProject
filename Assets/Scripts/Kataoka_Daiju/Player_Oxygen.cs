using System;
using UnityEngine;

public class Player_Oxygen : MonoBehaviour
{
    private float OxygenRate;
    public Player_OxygenGauge playerOxy;
    public float MaxOxygen = 100f;
    public float CurrentOxygen;
    void Awake()
    {

    }
    void Start()
    {
        CurrentOxygen = MaxOxygen;
    }
    private void SetOxygen(float Amount)
    {
        if (0 > Amount) return;
        if (CurrentOxygen < 0) return;
        CurrentOxygen = Mathf.Clamp(CurrentOxygen - Amount, 0f, MaxOxygen);
        OxygenRate = CurrentOxygen / MaxOxygen;
        playerOxy.TakeOxygenGaugeDamage(OxygenRate);
        if (CurrentOxygen < 0)
        {

        }
    }
    public void TakeOxygenDamage(float Damege)
    {
        SetOxygen(Damege);
    }
}