using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;         // プレイヤーのTransform
    public float searchRadius = 10f; // プレイヤーに気づく距離
    public float wanderRadius = 15f; // 徘徊範囲
    public float wanderInterval = 5f; // 徘徊時の地点更新間隔
    public float lostTimerLimit = 5f; // 視界から消えてから諦めるまでの時間

    private NavMeshAgent agent;
    private float wanderTimer;
    private float lostTimer;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // 1. プレイヤーが射程圏内、かつ障害物に隠れていないか確認
        if (distance < searchRadius && CanSeePlayer())
        {
            StartChasing();
        }
        else if (isChasing)
        {
            // 2. 追跡中だが視界から外れた場合、カウントダウン開始
            lostTimer += Time.deltaTime;
            if (lostTimer >= lostTimerLimit)
            {
                StopChasing();
            }
            else
            {
                // まだ諦めていないので、最後に見たプレイヤーの位置へ向かう
                agent.SetDestination(player.position);
            }
        }
        else
        {
            // 3. 徘徊モード
            DoWander();
        }
    }

    // 壁などを突き抜けて見ていないかチェック
    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;
        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, searchRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void StartChasing()
    {
        isChasing = true;
        lostTimer = 0;
        agent.speed = 5.0f; // 追跡時は少し速くする
        agent.SetDestination(player.position);
    }

    void StopChasing()
    {
        isChasing = false;
        lostTimer = 0;
        agent.speed = 3.5f; // 徘徊速度に戻す
        wanderTimer = wanderInterval; // すぐに次の徘徊地点を探す
    }

    void DoWander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            agent.SetDestination(RandomNavMeshLocation(wanderRadius));
            wanderTimer = 0;
        }
    }

    public Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            return hit.position;
        }
        return transform.position;
    }
}
