using UnityEngine;

public class PreDeathSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        [Header("Spawn Settings")]
        public GameObject[] prefabs;
        [Min(1)] public int spawnCount = 1;

        [Header("Position / Rotation")]
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffsetEuler = Vector3.zero;
        public bool useWorldOffset = true;

        [Header("Parent")]
        public bool setParent = false;
        public Transform parent;

        [Header("Random")]
        public bool randomPrefab = true;

        [Header("Scatter")]
        public bool randomScatter = false;
        public Vector3 scatterRange = Vector3.zero;
    }

    [Header("Spawn List")]
    [SerializeField] private SpawnEntry[] spawnEntries;

    [Header("Safety")]
    [SerializeField] private bool spawnOnlyOnce = true;

    private bool hasSpawned = false;

    /// <summary>
    /// 死亡前など、好きなタイミングで外部から呼ぶ用
    /// </summary>
    public void SpawnNow()
    {
        if (spawnOnlyOnce && hasSpawned)
            return;

        if (spawnEntries == null || spawnEntries.Length == 0)
            return;

        hasSpawned = true;

        foreach (var entry in spawnEntries)
        {
            SpawnEntryObjects(entry);
        }
    }

    /// <summary>
    /// 再度スポーン可能にしたい時用
    /// </summary>
    public void ResetSpawnFlag()
    {
        hasSpawned = false;
    }

    private void SpawnEntryObjects(SpawnEntry entry)
    {
        if (entry == null || entry.prefabs == null || entry.prefabs.Length == 0)
            return;

        for (int i = 0; i < entry.spawnCount; i++)
        {
            GameObject prefabToSpawn = GetPrefab(entry);
            if (prefabToSpawn == null)
                continue;

            Vector3 spawnPos = GetSpawnPosition(entry);
            Quaternion spawnRot = GetSpawnRotation(entry);

            Transform parent = null;
            if (entry.setParent)
            {
                parent = entry.parent;
            }
            Debug.Log("kita");
            Instantiate(prefabToSpawn, spawnPos, spawnRot, parent);
        }
    }

    private GameObject GetPrefab(SpawnEntry entry)
    {
        if (entry.prefabs == null || entry.prefabs.Length == 0)
            return null;

        if (entry.randomPrefab)
        {
            int index = Random.Range(0, entry.prefabs.Length);
            return entry.prefabs[index];
        }

        return entry.prefabs[0];
    }

    private Vector3 GetSpawnPosition(SpawnEntry entry)
    {
        Vector3 basePos = transform.position;
        Vector3 offset = entry.positionOffset;

        if (entry.randomScatter)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-entry.scatterRange.x, entry.scatterRange.x),
                Random.Range(-entry.scatterRange.y, entry.scatterRange.y),
                Random.Range(-entry.scatterRange.z, entry.scatterRange.z)
            );

            offset += randomOffset;
        }

        if (entry.useWorldOffset)
        {
            return basePos + offset;
        }
        else
        {
            return transform.TransformPoint(offset);
        }
    }

    private Quaternion GetSpawnRotation(SpawnEntry entry)
    {
        Quaternion offsetRot = Quaternion.Euler(entry.rotationOffsetEuler);
        return transform.rotation * offsetRot;
    }
}