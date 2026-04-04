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
        float fill = time / maxtime;
        player_gauge.fillAmount = fill;
        if (fill > 0.5f)
        {
            player_gauge.color = Color.white;
        }
        else
        {
            float t = fill * 2f;
            player_gauge.color = Color.Lerp(Color.red, Color.yellow, t);
        }

    }
}