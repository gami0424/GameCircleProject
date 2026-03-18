using UnityEngine;
using UnityEngine.AI;

public class FleeingMonster : MonoBehaviour
{
    public Transform player;
    public float detectRadius = 10f;
    public float fleeDistance = 15f;
    public float lostTimerLimit = 5f;

    private NavMeshAgent agent;
    private float lostTimer;
    private bool isFleeing = false;

    // --- 追加分 ---
    private float updateTimer;
    private float updateInterval = 0.2f; // 0.2秒ごとに目的地を更新

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < detectRadius && CanSeePlayer())
        {
            isFleeing = true;
            lostTimer = 0;
            agent.speed = 6.0f;

            // 頻繁に更新しすぎないようにタイマー管理
            updateTimer += Time.deltaTime;
            if (updateTimer >= updateInterval)
            {
                FleeFromPlayer();
                updateTimer = 0;
            }
        }
        else if (isFleeing)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= lostTimerLimit)
            {
                StopFleeing();
            }
            else
            {
                // 視界から消えても少しの間は目的地を追い直す
                updateTimer += Time.deltaTime;
                if (updateTimer >= updateInterval)
                {
                    FleeFromPlayer();
                    updateTimer = 0;
                }
            }
        }
        else
        {
            DoWander();
        }
    }

    void FleeFromPlayer()
    {
        // 既に移動中で、まだ目的地に遠いなら、新しい命令を出さない
        if (agent.hasPath && agent.remainingDistance > 2.0f) return;

        Vector3 directionToMe = (transform.position - player.position).normalized;
        // 15m先の「しっかり遠い場所」を一度だけターゲットにする
        Vector3 fleePos = transform.position + directionToMe * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePos, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void DoWander()
    {
        // 移動中はタイマーを進めない、または目的地を上書きしない
        if (agent.hasPath && agent.remainingDistance > 0.5f) return;

        wanderTimer += Time.deltaTime;
        if (wanderTimer >= 5f)
        {
            // OnUnitSphere（表面）を使うことで、必ず10m離れた場所を狙う
            Vector3 randomDir = Random.onUnitSphere;
            randomDir.y = 0; // 高さは無視
            Vector3 randomPos = transform.position + randomDir * 10f;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            wanderTimer = 0;
        }
    }

    // --- (以下、前回同様のメソッド) ---
    void StopFleeing()
    {
        isFleeing = false;
        lostTimer = 0;
        agent.speed = 3.5f;
        agent.ResetPath(); // 一旦立ち止まってから徘徊へ
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 direction = player.position - (transform.position + Vector3.up);
        if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, detectRadius))
        {
            if (hit.transform.CompareTag("Player")) return true;
        }
        return false;
    }

    private float wanderTimer;

}