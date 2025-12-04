using UnityEngine;
using UnityEngine.AI;

public class Enemy_Ishi : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform target;

    private void Update()
    {
        agent.SetDestination(target.position);
    }
}