using UnityEngine;

public class TimedSpawnerCaller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PreDeathSpawner targetSpawner;

    [Header("Timing")]
    [SerializeField] private float interval = 5f;
    [SerializeField] private bool callOnStart = false;
    [SerializeField] private bool loop = true;

    private float timer;
    private bool isRunning = true;

    private void Awake()
    {
        if (targetSpawner == null)
        {
            targetSpawner = GetComponent<PreDeathSpawner>();
        }
    }

    private void Start()
    {
        if (callOnStart)
        {
            CallSpawn();
        }

        timer = 0f;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        if (targetSpawner == null)
            return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            CallSpawn();

            if (loop)
            {
                timer = 0f;
            }
            else
            {
                isRunning = false;
            }
        }
    }

    private void CallSpawn()
    {
        targetSpawner.SpawnNow();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public void RestartTimer()
    {
        timer = 0f;
        isRunning = true;
    }

    public void CallNow()
    {
        CallSpawn();
    }
}