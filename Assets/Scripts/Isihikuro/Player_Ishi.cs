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
    public Vector3 camLook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plInput = GetComponent<PlayerInput>();
        rd = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void FixedUpdate()
    {
        PlMove();
    }

    void PlMove()
    {
        float speedDash = 1;
        if (plInput.actions["Dash"].ReadValue<float>() > 0) speedDash = 1.5f;
        Vector2 move = plInput.actions["Move"].ReadValue<Vector2>() * speed * speedDash;
        Vector3 moveMain = move.x * cameraObj.transform.right + move.y * cameraObj.transform.forward;
        moveMain.y = rd.linearVelocity.y;
        rd.linearVelocity = moveMain;
    }
    
    void CameraMove()
    {
        Vector2 move = plInput.actions["Look"].ReadValue<Vector2>() * camSpeed * Time.deltaTime;

        camLook += new Vector3(-move.y, move.x, 0);

        //Y軸だけ視点に上限を設ける
        camLook.x = Mathf.Clamp(camLook.x, -90, 90);

        //カメラとプレイヤーの向きを動かす
        cameraObj.transform.rotation = Quaternion.Euler(camLook);
        //transform.rotation = Quaternion.Euler(camLook);
    }
}
