using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform player;

    [SerializeField] bool followAlways;
    bool follow = false;

    void Update()
    {
        if (follow)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (followAlways)
        {
            if (other.CompareTag("Player"))
            {
                agent.SetDestination(other.transform.position);
            }
        }
    }

    public void SetFollow(bool condition)
    {
        this.follow = condition;
    }
}
