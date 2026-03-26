using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FleeingMonsterAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;

    [Header("Detection / Movement")]
    [SerializeField] private float detectRadius = 10f;
    [SerializeField] private float fleeDistance = 15f;
    [SerializeField] private float wanderRadius = 10f;

    [Header("Timing")]
    [SerializeField] private float fleeRunDuration = 4f;
    [SerializeField] private float fleeIdleDuration = 2f;
    [SerializeField] private float wanderInterval = 5f;
    [SerializeField] private float fleeRepathInterval = 0.5f;

    [Header("Speed")]
    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float fleeSpeed = 6f;

    [Header("Distance Threshold")]
    [SerializeField] private float arriveDistance = 0.5f;
    [SerializeField] private float fleeArriveDistance = 1.5f;
    [SerializeField] private float minWanderMoveDistance = 1.0f;

    private NavMeshAgent agent;

    private float stateTimer;
    private float repathTimer;
    private float wanderTimer;

    private bool isPlayerNear;
    private bool isFleeingIdle;

    private readonly int speedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        TryFindPlayer();
    }

    private void Update()
    {
        if (!ValidateReferences())
        {
            UpdateAnimator();
            return;
        }

        Vector3 toPlayer = player.position - transform.position;
        float sqrDistance = toPlayer.sqrMagnitude;
        isPlayerNear = sqrDistance <= detectRadius * detectRadius;

        if (isPlayerNear)
        {
            HandleFleeing();
        }
        else
        {
            HandleWandering();
        }

        UpdateAnimator();
    }

    private bool ValidateReferences()
    {
        if (player == null)
        {
            TryFindPlayer();
            if (player == null) return false;
        }

        if (agent == null) return false;
        if (!agent.isOnNavMesh) return false;

        return true;
    }

    private void TryFindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    private void HandleFleeing()
    {
        // 徘徊関連はリセット
        wanderTimer = 0f;

        if (!isFleeingIdle)
        {
            stateTimer += Time.deltaTime;
            repathTimer += Time.deltaTime;

            agent.isStopped = false;
            agent.speed = fleeSpeed;

            bool arrived = !agent.pathPending &&
                           agent.hasPath &&
                           agent.remainingDistance <= fleeArriveDistance;

            if (!agent.hasPath || arrived || repathTimer >= fleeRepathInterval)
            {
                SetFleeDestination();
                repathTimer = 0f;
            }

            if (stateTimer >= fleeRunDuration)
            {
                isFleeingIdle = true;
                stateTimer = 0f;
                repathTimer = 0f;
                agent.isStopped = true;
                agent.ResetPath();
            }
        }
        else
        {
            stateTimer += Time.deltaTime;

            agent.isStopped = true;

            if (stateTimer >= fleeIdleDuration)
            {
                isFleeingIdle = false;
                stateTimer = 0f;
                repathTimer = 0f;
            }
        }
    }

    private void HandleWandering()
    {
        // 逃走状態を抜けたのでリセット
        isFleeingIdle = false;
        stateTimer = 0f;
        repathTimer = 0f;

        agent.isStopped = false;
        agent.speed = wanderSpeed;

        wanderTimer += Time.deltaTime;

        bool arrived = !agent.pathPending &&
                       agent.hasPath &&
                       agent.remainingDistance <= arriveDistance;

        if (!agent.hasPath || arrived || wanderTimer >= wanderInterval)
        {
            SetRandomWanderDestination();
            wanderTimer = 0f;
        }
    }

    private void SetRandomWanderDestination()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
            randomDir.y = 0f;

            Vector3 candidate = transform.position + randomDir;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                float sqrMoveDist = (hit.position - transform.position).sqrMagnitude;
                if (sqrMoveDist >= minWanderMoveDistance * minWanderMoveDistance)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        // 候補が取れなかった時は何もしない
    }

    private void SetFleeDestination()
    {
        if (player == null) return;

        Vector3 baseDir = transform.position - player.position;
        baseDir.y = 0f;

        if (baseDir.sqrMagnitude < 0.001f)
        {
            baseDir = transform.forward;
        }

        baseDir.Normalize();

        float[] angles = { 0f, 30f, -30f, 60f, -60f, 90f, -90f, 135f, -135f, 180f };

        Vector3 bestPos = transform.position;
        bool found = false;
        float bestScore = float.MinValue;

        float currentPlayerDistSqr = (transform.position - player.position).sqrMagnitude;

        foreach (float angle in angles)
        {
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            Vector3 rawTarget = transform.position + dir * fleeDistance;

            if (!NavMesh.SamplePosition(rawTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                continue;

            NavMeshPath path = new NavMeshPath();
            if (!agent.CalculatePath(hit.position, path))
                continue;

            if (path.status != NavMeshPathStatus.PathComplete)
                continue;

            float playerDistSqr = (hit.position - player.position).sqrMagnitude;
            if (playerDistSqr <= currentPlayerDistSqr)
                continue;

            float score = playerDistSqr;

            if (score > bestScore)
            {
                bestScore = score;
                bestPos = hit.position;
                found = true;
            }
        }

        if (found)
        {
            agent.SetDestination(bestPos);
        }
        else
        {
            // 最後の保険：徘徊先を使ってでも場所を変える
            SetRandomWanderDestination();
        }
    }

    private void UpdateAnimator()
    {
        if (anim == null || agent == null) return;

        float speed = 0f;

        if (agent.isOnNavMesh && !agent.isStopped)
        {
            speed = agent.velocity.magnitude;
        }

        anim.SetFloat(speedHash, speed);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
#endif
}