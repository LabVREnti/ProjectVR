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
    bool stunned = false;

    float timer = 3.0f;

    [SerializeField] Material stunnedMat;
    [SerializeField] Material initMat;
    void Update()
    {
        if(stunned)
        {
            follow = false;
            timer -= Time.deltaTime;
            this.GetComponent<MeshRenderer>().material = initMat;
            this.GetComponent<MeshRenderer>().material = stunnedMat;

            if(timer <= 0.0f) { 
                follow = true;
                timer = 3.0f;
                this.GetComponent<MeshRenderer>().material = initMat;
                stunned = false;
            }
            Debug.Log("stunneado");
        }
        else { Debug.Log("vuelta al follow"); }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Torch"))
        {
            // Play sonido de hacer daño al ogro y animacion daño ogro
            stunned = true;
            Debug.Log("te he pegado");
        }

    }

    public void SetFollow(bool condition)
    {
        this.follow = condition;
    }
}
