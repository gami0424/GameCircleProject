using UnityEngine;
using System.Collections;

public class RobotAdvancedController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float shakeIntensity = 10.0f;
    private bool isMoving = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMoving)
        {
            // 2Dの場合はVector2.up、3Dの場合はVector3.forwardなど
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }

    public void StartSequence()
    {
        StartCoroutine(RobotActionRoutine());
    }

    IEnumerator RobotActionRoutine()
    {
        float elapsed = 0f;
        Quaternion originalRotation = transform.rotation;

        // 1. 3秒間揺れる
        while (elapsed < 3.0f)
        {
            float zRotation = Mathf.Sin(Time.time * 30f) * shakeIntensity;
            transform.rotation = originalRotation * Quaternion.Euler(0, 0, zRotation);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 揺れリセット & 180度反転
        transform.rotation = originalRotation;
        transform.Rotate(0, 0, 180f);

        // 2. アニメーション開始
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }

        // 3. 前進フラグON
        isMoving = true;
    } // ← ここでコルーチンの終わり

    // OnTriggerEnterはメソッドの外に出す
    private void OnTriggerEnter2D(Collider2D other) // 2Dの場合はこっち
    {
        if (other.CompareTag("StopPoint"))
        {
            StopRobot();
        }
    }

    // 3Dの場合はこちらを使います
    private void OnTriggerEnter(Collider other)
    {
        // 触れた相手のタグが StopPoint かどうか判定
        if (other.CompareTag("StopPoint"))
        {
            Debug.Log("停止ポイントに接触しました！"); // 動作確認用
            isMoving = false;

            if (anim != null)
            {
                anim.SetBool("isWalking", false);
            }
        }
    }

    private void StopRobot()
    {
        isMoving = false;
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }
    }
}




