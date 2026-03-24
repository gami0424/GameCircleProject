using UnityEngine;
using UnityEngine.AI;

public class Enemy_Ishi : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;

    public float testTime = 3;

    private void Update()
    {
        // testTime -= Time.deltaTime;
        // if (testTime <= 0)
        // {
        //     agent.isStopped = true;
        //     testTime = 0;
        // }

        agent.SetDestination(target.position);
    }
}