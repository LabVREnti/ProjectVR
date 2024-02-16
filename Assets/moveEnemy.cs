using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    CapsuleCollider col;

    [SerializeField] bool followAlways;
   // [SerializeField] bool followByCloseness;
    bool follow;
   [SerializeField] bool stunned;

    float timer = 4.0f;

    Animator anim;
    Rigidbody rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindAnyObjectByType<playerController>().GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        follow = false;
        stunned = false;
    }
    void Update()
    {
        if(stunned)
        {
            follow = false;
            rb.velocity = Vector3.zero;
            timer -= Time.deltaTime;
            anim.SetBool("Walk", false);
            anim.SetBool("Die", true);
            agent.enabled = false;

            if (timer <= 2.0f) { 
               
                anim.SetBool("Die", false);
                anim.SetBool("Walk", true);

                if (timer <= 0.0f)
                {
                    timer = 4.0f;
                    agent.enabled = true;
                    stunned = false;
                    follow = true;
                }
            }
            Debug.Log("stunneado");
        }
        else { Debug.Log("vuelta al follow"); }

        if (follow)
        {
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk", true);
        }

        if (followAlways)
        {    
            agent.SetDestination(player.transform.position);
            anim.SetBool("Walk", true);       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  if (followByCloseness)
          //  {
                follow = true;
                //setfollow true huevona
                anim.SetBool("Walk", true);
          //  }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  if (followByCloseness)
         //   {
                follow = false;
         //   }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Torch"))
        {
            // Play sonido de hacer daño al ogro y animacion daño ogro
            Debug.Log("te he pegado");
            stunned = true;
        }

    }

    public void SetFollow(bool condition)
    {
        this.follow = condition;
    }
}
