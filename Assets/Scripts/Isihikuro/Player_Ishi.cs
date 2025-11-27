using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Ishi : MonoBehaviour
{
    PlayerInput plInput;
    Rigidbody rd;

    public GameObject cameraObj;

    public float speed = 10;
    public float camSpeed = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plInput = GetComponent<PlayerInput>();
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlMove();
        CameraMove();
    }

    void PlMove()
    {
        Vector2 move = plInput.actions["Move"].ReadValue<Vector2>() * speed;
        rd.linearVelocity = new Vector3(move.x, 0, move.y);
    }
    
    void CameraMove()
    {
        Vector2 move = plInput.actions["Look"].ReadValue<Vector2>() * camSpeed;
        Debug.Log(move);
        Vector3 ang = cameraObj.transform.eulerAngles;
        ang.x = 0;// Mathf.Clamp(ang.x+move.y, -89, 89);
        ang.y = move.x;
        cameraObj.transform.localEulerAngles = ang;
    }
}
