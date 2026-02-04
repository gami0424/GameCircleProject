using System.Collections;
using UnityEngine;

public class SpawnPrefabAfterExit : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // 元のプレハブ
    [SerializeField] private float delay = 0.5f;
    public float Takasa = 1f;

    [SerializeField] private GameObject temporaryPrefab; // 追加：2秒だけ出すプレハブ

    private Coroutine spawnCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnCoroutine = StartCoroutine(SpawnAfterDelay());
        }
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        // 元のプレハブは今まで通り設置
        Instantiate(prefab, transform.position, Quaternion.identity);

        // 新しいPrefabはトリガーオブジェクトの上に生成して2秒後に消す
        //仮として作ったものなので一旦無効にしてます
        // if (temporaryPrefab != null)
        // {
        //     Vector3 spawnPosition = transform.position + Vector3.up * Takasa;
        //     GameObject tempInstance = Instantiate(temporaryPrefab, spawnPosition, Quaternion.identity);
        //     Destroy(tempInstance, 2f);
        // }

        spawnCoroutine = null;
    }
}


