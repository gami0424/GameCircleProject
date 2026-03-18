using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class metaaar : MonoBehaviour
{

    [SerializeField] private float time = 300f;
    [SerializeField] private float maxtime = 300f;
    private Image player_gauge;
    private float gauge;


    void Start()
    {
        player_gauge = GetComponent<Image>();
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        player_gauge.fillAmount = time / maxtime;
        if (gauge >= 1) gauge = 0;
    }
}