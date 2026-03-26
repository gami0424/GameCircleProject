using UnityEngine;
using UnityEngine.AI;

public class DrillManChaseController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackDistance = 2.2f;
    [SerializeField] private float normalSpeed = 4.5f;
    [SerializeField] private float slowSpeed = 1.5f;

    private bool isActive = false;
    private bool isTemporarilyBlocked = false;
    private SlowTrapWall currentTrapWall;

    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        DebugCheckNavMesh("Start");

        if (agent == null) return;
        if (!agent.enabled) return;

      
        TrySnapToNavMesh(3f);

        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.speed = normalSpeed;
        }
    }

    private void Update()
    {
        if (!isActive || targetPlayer == null || agent == null) return;
        if (!agent.enabled) return;

        if (!agent.isOnNavMesh)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            return;
        }

        if (isTemporarilyBlocked)
        {
            agent.isStopped = true;

            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }

            if (currentTrapWall != null)
            {
                float distToWall = Vector3.Distance(transform.position, currentTrapWall.transform.position);
                if (distToWall <= attackDistance)
                {
                    AttackTrapWall();
                }
            }

            return;
        }

        agent.isStopped = false;
        agent.SetDestination(targetPlayer.position);

        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    public void ActivateChase(Transform player)
    {
        targetPlayer = player;
        isActive = true;

        DebugCheckNavMesh("ActivateChase ‘O");

        if (agent == null)
        {
            Debug.LogError("agent ‚ª null");
            return;
        }

        if (!agent.enabled)
        {
            Debug.LogError("agent.enabled ‚ª false");
            return;
        }

       
        TrySnapToNavMesh(3f);

        DebugCheckNavMesh("ActivateChase •â³Œã");

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("agent.isOnNavMesh ‚ª false ‚È‚Ì‚Å’ÇÕŠJŽn‚µ‚È‚¢");
            return;
        }

        agent.speed = normalSpeed;
        agent.isStopped = false;
        agent.SetDestination(targetPlayer.position);
    }

    public void SetLowSpeedMode(bool enable)
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;
        agent.speed = enable ? slowSpeed : normalSpeed;
    }

    public void BlockByTrap(SlowTrapWall wall)
    {
        currentTrapWall = wall;
        isTemporarilyBlocked = true;
    }

    public void ReleaseFromTrap()
    {
        currentTrapWall = null;
        isTemporarilyBlocked = false;
    }

    private void AttackTrapWall()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (currentTrapWall != null)
        {
            currentTrapWall.BreakWall();
        }

        ReleaseFromTrap();
    }

    private bool TrySnapToNavMesh(float maxDistance)
    {
        if (agent == null || !agent.enabled) return false;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, maxDistance, NavMesh.AllAreas))
        {
            bool warped = agent.Warp(hit.position);
            Debug.Log("TrySnapToNavMesh / warped = " + warped + " / pos = " + hit.position);
            return warped;
        }

        Debug.LogWarning("TrySnapToNavMesh / ‹ß‚­‚ÉNavMesh‚ªŒ©‚Â‚©‚ç‚È‚¢");
        return false;
    }

    private void DebugCheckNavMesh(string label)
    {
        if (agent == null)
        {
            Debug.LogError(label + " / agent null");
            return;
        }

        Debug.Log(label + " / transform.position = " + transform.position);
        Debug.Log(label + " / agent.enabled = " + agent.enabled);
        Debug.Log(label + " / agent.isOnNavMesh = " + agent.isOnNavMesh);
        Debug.Log(label + " / baseOffset = " + agent.baseOffset);
        Debug.Log(label + " / radius = " + agent.radius);
        Debug.Log(label + " / height = " + agent.height);

        NavMeshHit hit;
        bool found = NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas);
        Debug.Log(label + " / SamplePosition found = " + found);

        if (found)
        {
            Debug.Log(label + " / nearest navmesh pos = " + hit.position);
        }
    }
}