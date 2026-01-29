using System.Collections;
using UnityEngine;

public class SpawnPrefabAfterExit : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // 置きたいプレハブ
    [SerializeField] private float delay = 0.5f;

    private Coroutine spawnCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 再侵入時に前の処理があればキャンセル
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
        Instantiate(prefab, transform.position, Quaternion.identity);
        spawnCoroutine = null;
    }
}

