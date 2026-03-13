using UnityEngine;
using System.Collections;

public class DrilmanAdvancedController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float shakeIntensity = 10.0f; // 揺れの強さ
    public float shakeSpeed = 50.0f;    // 揺れの速さ

    private bool isMoving = false;
    private bool isWaiting = false;
    private bool isCollapsed = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 移動フラグON、かつ待ち伏せ中でも倒れてもいない時だけ前進
        if (isMoving && !isWaiting && !isCollapsed)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void StartSequence()
    {
        if (!isMoving && !isCollapsed)
        {
            StartCoroutine(RobotActionRoutine());
        }
    }

    IEnumerator RobotActionRoutine()
    {
        // --- 1. 3秒間、左右に小刻みに揺れる（復活！） ---
        float elapsed = 0f;
        Quaternion originalRotation = transform.rotation;

        while (elapsed < 3.0f)
        {
            // Y軸を高速で往復させる
            float yRotation = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            transform.rotation = originalRotation * Quaternion.Euler(0, yRotation, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 揺れをリセットして180度反転
        transform.rotation = originalRotation;
        transform.Rotate(0, 180f, 0);

        // --- 2. アニメーション開始 ---
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }

        isMoving = true;
        Debug.Log("ロボットが前進を開始しました");
    }

    // ストップポイントや天井に触れた時の判定
    private void OnTriggerEnter(Collider other)
    {
        // ストップポイント（タグ設定が必要）
        if (other.CompareTag("StopPoint"))
        {
            StopAtPosition();
        }

        // 天井（タグ設定が必要）
        if (other.CompareTag("Ceiling"))
        {
            Collapse();
        }
    }

    public void StopAtPosition()
    {
        if (isWaiting) return;

        isWaiting = true;
        if (anim != null) anim.SetBool("isWalking", false);
        Debug.Log("天井の下で停止。ガス缶を待っています。");
    }

    public void Collapse()
    {
        if (isCollapsed) return;
        StartCoroutine(CollapseRoutine());
    }

    IEnumerator CollapseRoutine()
    {
        isCollapsed = true;
        if (anim != null) anim.SetBool("isWalking", false);

        float elapsed = 0f;
        float duration = 0.8f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(90, 0, 0);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
        Debug.Log("ロボットが圧壊しました。");
    }
}