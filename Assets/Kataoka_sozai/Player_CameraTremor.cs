using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Player_CameraTremor : MonoBehaviour
{
    // 揺れの時間と強さのデフォルト値
    [SerializeField] float defaultDuration = 0.2f; //カメラ揺らす時間
    [SerializeField] float defaultMagnitude = 1f; //カメラ揺らす強さ
    private bool NowShake = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (NowShake == true) return;
            Debug.Log("カメラ揺らすよ");
            Shake();
        }
    }
    public void Shake()
    {
        StartCoroutine(DoShake(defaultDuration, defaultMagnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        NowShake = true;
        Vector3 originalPos = transform.localPosition; //originalPosにカメラの元の位置を保存する
        float elapsed = 0.0f;  //経過時間測る用

        while (elapsed < duration) //経過時間が揺らす時間未満の間実っっっっっ行
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(1.5f, 2f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            // 次のフレームまで待機
            yield return null;
        }
        Debug.Log("揺らし終わったよ");
        transform.localPosition = originalPos; //揺らし終えたらカメラ元の位置戻す
        NowShake = false;
    }
}