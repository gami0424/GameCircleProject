using Unity.VisualScripting;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private float SlowSpeed;
    float defaultSpeed;

    public float slow = 0.2f;

    public Collider coll;

    public bool isSlow = false;

    private Player_CharacterController_FPS PCCFPS;
    private void Awake()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;

    }


    private void Start()
    {

    }

    private void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (isSlow == true) return;
        if (PCCFPS == null && other.CompareTag("Player"))
        {
            Debug.Log("減速しました。");
            isSlow = true;
            PCCFPS = other.GetComponentInParent<Player_CharacterController_FPS>();
            defaultSpeed = PCCFPS.moveSpeed;

            SlowSpeed = PCCFPS.moveSpeed * slow;

            PCCFPS.moveSpeed = SlowSpeed;
        }
        else if (other.CompareTag("Player"))
        {
            isSlow = true;
            PCCFPS = other.GetComponentInParent<Player_CharacterController_FPS>();
            defaultSpeed = PCCFPS.moveSpeed;

            SlowSpeed = PCCFPS.moveSpeed * slow;

            PCCFPS.moveSpeed = SlowSpeed;
        }








        /*
        if (other.CompareTag("SlowArea"))
        {
            SlowArea area = other.GetComponent<SlowArea>();
            if (area != null)
            {
                moveSpeed = defaultSpeed * area.slowMultiplier;
            }
        }*/
    }

    void OnTriggerExit(Collider other)
    {/*
        if (other.CompareTag("SlowArea"))
        {
            moveSpeed = defaultSpeed;
        }*/

        PCCFPS.moveSpeed = defaultSpeed;
        isSlow = false;
    }
}